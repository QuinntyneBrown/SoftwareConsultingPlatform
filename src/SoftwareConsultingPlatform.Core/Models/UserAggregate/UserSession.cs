namespace SoftwareConsultingPlatform.Core.Models.UserAggregate;

/// <summary>
/// User session tracking across devices.
/// </summary>
public class UserSession
{
    public Guid UserSessionId { get; set; }
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public string? DeviceInfo { get; set; }
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUsedAt { get; set; }

    // Navigation property
    public User User { get; set; } = null!;
}
