namespace SoftwareConsultingPlatform.Identity.Core.Aggregates;

public class UserSession
{
    public Guid UserSessionId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid UserId { get; private set; }
    public string? DeviceInfo { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime LastUsedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }

    public User? User { get; private set; }

    public bool IsActive => !EndedAt.HasValue;

    private UserSession() { }

    public UserSession(Guid tenantId, Guid userId, string? deviceInfo, string? ipAddress, string? userAgent)
    {
        UserSessionId = Guid.NewGuid();
        TenantId = tenantId;
        UserId = userId;
        DeviceInfo = deviceInfo;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        CreatedAt = DateTime.UtcNow;
        LastUsedAt = DateTime.UtcNow;
    }

    public void UpdateLastUsed()
    {
        LastUsedAt = DateTime.UtcNow;
    }

    public void End()
    {
        EndedAt = DateTime.UtcNow;
    }
}
