using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SoftwareConsultingPlatform.Identity.Core.Aggregates;
using SoftwareConsultingPlatform.Identity.Core.ValueObjects;
using SoftwareConsultingPlatform.Identity.Infrastructure.Data;
using Shared.Core.Interfaces;
using Shared.Messages.Events.Identity;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse?>
{
    private readonly IdentityDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IdentityDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        ILogger<LoginCommandHandler> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<LoginResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.TenantId == _tenantContext.TenantId && u.Email == request.Email, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("User not found: {Email}", request.Email);
            return null;
        }

        if (user.IsLocked)
        {
            _logger.LogWarning("User account is locked: {Email}", request.Email);
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            user.RecordFailedLogin();
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogWarning("Invalid password for user: {Email}", request.Email);
            return null;
        }

        if (!user.EmailVerified)
        {
            _logger.LogWarning("Email not verified for user: {Email}", request.Email);
            return null;
        }

        // Generate tokens
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();
        var accessTokenExpiry = DateTime.UtcNow.AddMinutes(
            _configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15));
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(
            _configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays", 7));

        // Store refresh token
        var refreshTokenEntity = new SoftwareConsultingPlatform.Identity.Core.Aggregates.RefreshToken(
            user.TenantId,
            user.UserId,
            refreshToken,
            refreshTokenExpiry);

        _context.RefreshTokens.Add(refreshTokenEntity);

        // Record login
        user.RecordLogin();

        // Create session
        var httpContext = _httpContextAccessor.HttpContext;
        var session = new UserSession(
            user.TenantId,
            user.UserId,
            httpContext?.Request.Headers["User-Agent"].FirstOrDefault(),
            httpContext?.Connection.RemoteIpAddress?.ToString(),
            httpContext?.Request.Headers["User-Agent"].FirstOrDefault());

        _context.UserSessions.Add(session);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        await _publishEndpoint.Publish(new UserLoggedInEvent(
            user.UserId,
            user.TenantId,
            httpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            httpContext?.Request.Headers["User-Agent"].FirstOrDefault() ?? "unknown",
            DateTime.UtcNow
        ), cancellationToken);

        _logger.LogInformation("Login successful for user: {Email}", request.Email);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = accessTokenExpiry,
            User = new UserInfo
            {
                UserId = user.UserId,
                Email = user.Email,
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl,
                EmailVerified = user.EmailVerified,
                MfaEnabled = user.MfaEnabled
            }
        };
    }

    private string GenerateAccessToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var secretKey = jwtSettings["Secret"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("tenant_id", user.TenantId.ToString()),
            new("full_name", user.FullName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"] ?? "SoftwareConsultingPlatform",
            audience: jwtSettings["Audience"] ?? "SoftwareConsultingPlatform",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
