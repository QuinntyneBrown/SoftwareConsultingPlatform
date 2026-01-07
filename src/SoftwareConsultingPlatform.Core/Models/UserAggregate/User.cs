using SoftwareConsultingPlatform.Core.Models.UserAggregate.Enums;

namespace SoftwareConsultingPlatform.Core.Models.UserAggregate;

/// <summary>
/// User aggregate root representing a user account within a tenant.
/// </summary>
public class User
{
    public Guid UserId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public string? CompanyName { get; private set; }

    // Security
    public bool EmailVerified { get; private set; }
    public string? EmailVerificationToken { get; private set; }
    public string? PasswordResetToken { get; private set; }
    public string? MfaSecret { get; private set; }
    public bool MfaEnabled { get; private set; }

    // Status
    public UserStatus Status { get; private set; }
    public DateTime? LockedUntil { get; private set; }

    // Metadata
    public string? AvatarUrl { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public int LoginCount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Private constructor for EF Core
    private User() { }

    public User(
        Guid tenantId,
        string email,
        string passwordHash,
        string fullName)
    {
        UserId = Guid.NewGuid();
        TenantId = tenantId;
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        Status = UserStatus.Pending;
        EmailVerified = false;
        MfaEnabled = false;
        LoginCount = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void VerifyEmail()
    {
        EmailVerified = true;
        EmailVerificationToken = null;
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetEmailVerificationToken(string token)
    {
        EmailVerificationToken = token;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPasswordResetToken(string token)
    {
        PasswordResetToken = token;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        PasswordResetToken = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(string? fullName, string? phone, string? companyName)
    {
        if (!string.IsNullOrWhiteSpace(fullName))
            FullName = fullName;
        Phone = phone;
        CompanyName = companyName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAvatar(string? avatarUrl)
    {
        AvatarUrl = avatarUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        LoginCount++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Lock(TimeSpan duration)
    {
        Status = UserStatus.Locked;
        LockedUntil = DateTime.UtcNow.Add(duration);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unlock()
    {
        Status = UserStatus.Active;
        LockedUntil = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void EnableMfa(string mfaSecret)
    {
        MfaSecret = mfaSecret ?? throw new ArgumentNullException(nameof(mfaSecret));
        MfaEnabled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DisableMfa()
    {
        MfaSecret = null;
        MfaEnabled = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
        Status = UserStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        DeletedAt = null;
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }
}
