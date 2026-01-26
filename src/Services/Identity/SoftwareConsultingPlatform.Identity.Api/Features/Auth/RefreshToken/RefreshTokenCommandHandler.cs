using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SoftwareConsultingPlatform.Identity.Infrastructure.Data;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse?>
{
    private readonly IdentityDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IdentityDbContext context,
        IConfiguration configuration,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<RefreshTokenResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

        if (refreshToken == null || !refreshToken.IsActive || refreshToken.User == null)
        {
            _logger.LogWarning("Invalid refresh token attempt");
            return null;
        }

        var user = refreshToken.User;

        // Revoke old token
        var newRefreshTokenString = GenerateRefreshToken();
        refreshToken.Revoke(newRefreshTokenString);

        // Create new refresh token
        var newRefreshToken = new Core.Aggregates.RefreshToken(
            user.TenantId,
            user.UserId,
            newRefreshTokenString,
            DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays", 7)));

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        // Generate new access token
        var accessToken = GenerateAccessToken(user);
        var accessTokenExpiry = DateTime.UtcNow.AddMinutes(
            _configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15));

        return new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshTokenString,
            ExpiresAt = accessTokenExpiry
        };
    }

    private string GenerateAccessToken(Core.Aggregates.User user)
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
