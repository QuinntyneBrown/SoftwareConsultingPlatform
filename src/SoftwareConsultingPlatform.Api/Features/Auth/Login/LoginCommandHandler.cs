using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;
using SoftwareConsultingPlatform.Core.Models.UserAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SoftwareConsultingPlatform.Api.Features.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ISoftwareConsultingPlatformContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        ISoftwareConsultingPlatformContext context,
        ITenantContext tenantContext,
        IConfiguration configuration,
        ILogger<LoginCommandHandler> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.TenantId == _tenantContext.TenantId && u.Email == request.Email, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Failed login attempt for email: {Email}", request.Email);
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        if (!user.EmailVerified)
        {
            throw new UnauthorizedAccessException("Email not verified. Please verify your email before logging in.");
        }

        // Record login
        user.RecordLogin();
        await _context.SaveChangesAsync(cancellationToken);

        // Generate JWT access token
        var accessToken = GenerateAccessToken(user);

        // Generate refresh token
        var refreshToken = Guid.NewGuid().ToString("N");
        var refreshTokenEntity = new RefreshToken(user.UserId, _tenantContext.TenantId, refreshToken, DateTime.UtcNow.AddDays(7));
        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User logged in successfully: {UserId}", user.UserId);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                FullName = user.FullName,
                EmailVerified = user.EmailVerified,
                MfaEnabled = user.MfaEnabled
            }
        };
    }

    private string GenerateAccessToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secret = jwtSettings["Secret"] ?? "your-secret-key-min-32-characters-long-for-security";
        var issuer = jwtSettings["Issuer"] ?? "SoftwareConsultingPlatform";
        var audience = jwtSettings["Audience"] ?? "SoftwareConsultingPlatform";

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("tenant_id", user.TenantId.ToString()),
            new Claim("full_name", user.FullName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
