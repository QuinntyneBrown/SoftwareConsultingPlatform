namespace SoftwareConsultingPlatform.Services.Core.Aggregates;

public class ServiceInquiry
{
    public Guid ServiceInquiryId { get; private set; }
    public Guid ServiceId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Company { get; private set; }
    public string ProjectDescription { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public Service? Service { get; private set; }

    private ServiceInquiry() { }

    public ServiceInquiry(Guid serviceId, Guid tenantId, string name, string email, string? company, string projectDescription)
    {
        ServiceInquiryId = Guid.NewGuid();
        ServiceId = serviceId;
        TenantId = tenantId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Company = company;
        ProjectDescription = projectDescription ?? throw new ArgumentNullException(nameof(projectDescription));
        CreatedAt = DateTime.UtcNow;
    }
}
