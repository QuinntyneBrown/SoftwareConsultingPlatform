namespace SoftwareConsultingPlatform.Core.Models.UserAggregate;

/// <summary>
/// Refresh token for JWT authentication.
/// </summary>
public class RefreshToken
{
    public Guid RefreshTokenId { get; set; }
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;

    // Navigation property
    public User User { get; set; } = null!;

    // Private constructor for EF Core
    private RefreshToken() { }

    public RefreshToken(Guid userId, Guid tenantId, string token, DateTime expiresAt)
    {
        RefreshTokenId = Guid.NewGuid();
        UserId = userId;
        TenantId = tenantId;
        Token = token ?? throw new ArgumentNullException(nameof(token));
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
    }

    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }
}
