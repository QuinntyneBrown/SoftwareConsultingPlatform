using SoftwareConsultingPlatform.Identity.Core.ValueObjects;

namespace SoftwareConsultingPlatform.Identity.Core.Aggregates;

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
    public DateTime? PasswordResetTokenExpiry { get; private set; }
    public string? MfaSecret { get; private set; }
    public bool MfaEnabled { get; private set; }

    // Status
    public UserStatus Status { get; private set; }
    public DateTime? LockedUntil { get; private set; }
    public int FailedLoginAttempts { get; private set; }

    // Metadata
    public string? AvatarUrl { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public int LoginCount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Navigation properties
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();
    public ICollection<UserSession> Sessions { get; private set; } = new List<UserSession>();
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    public ICollection<ActivityLog> ActivityLogs { get; private set; } = new List<ActivityLog>();

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
        FailedLoginAttempts = 0;
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

    public void SetPasswordResetToken(string token, TimeSpan expiry)
    {
        PasswordResetToken = token;
        PasswordResetTokenExpiry = DateTime.UtcNow.Add(expiry);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool ValidatePasswordResetToken(string token)
    {
        return PasswordResetToken == token &&
               PasswordResetTokenExpiry.HasValue &&
               PasswordResetTokenExpiry.Value > DateTime.UtcNow;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        PasswordResetToken = null;
        PasswordResetTokenExpiry = null;
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
        FailedLoginAttempts = 0;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordFailedLogin()
    {
        FailedLoginAttempts++;
        UpdatedAt = DateTime.UtcNow;

        // Lock after 5 failed attempts
        if (FailedLoginAttempts >= 5)
        {
            Lock(TimeSpan.FromMinutes(15));
        }
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
        FailedLoginAttempts = 0;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsLocked => Status == UserStatus.Locked && LockedUntil.HasValue && LockedUntil.Value > DateTime.UtcNow;

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
