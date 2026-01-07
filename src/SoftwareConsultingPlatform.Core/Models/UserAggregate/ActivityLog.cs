namespace SoftwareConsultingPlatform.Core.Models.UserAggregate;

/// <summary>
/// Activity log for tracking user actions.
/// </summary>
public class ActivityLog
{
    public Guid ActivityLogId { get; set; }
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Resource { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime Timestamp { get; set; }

    // Navigation property
    public User User { get; set; } = null!;
}
