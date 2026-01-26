namespace SoftwareConsultingPlatform.Identity.Core.Aggregates;

public class ActivityLog
{
    public Guid ActivityLogId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid UserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string? Resource { get; private set; }
    public string? ResourceId { get; private set; }
    public string? Details { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public DateTime Timestamp { get; private set; }

    public User? User { get; private set; }

    private ActivityLog() { }

    public ActivityLog(
        Guid tenantId,
        Guid userId,
        string action,
        string? resource = null,
        string? resourceId = null,
        string? details = null,
        string? ipAddress = null,
        string? userAgent = null)
    {
        ActivityLogId = Guid.NewGuid();
        TenantId = tenantId;
        UserId = userId;
        Action = action ?? throw new ArgumentNullException(nameof(action));
        Resource = resource;
        ResourceId = resourceId;
        Details = details;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        Timestamp = DateTime.UtcNow;
    }
}
