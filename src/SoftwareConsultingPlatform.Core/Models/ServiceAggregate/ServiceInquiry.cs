namespace SoftwareConsultingPlatform.Core.Models.ServiceAggregate;

/// <summary>
/// Service inquiry submission from potential clients.
/// </summary>
public class ServiceInquiry
{
    public Guid ServiceInquiryId { get; set; }
    public Guid ServiceId { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string ProjectDescription { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public Service Service { get; set; } = null!;

    // Private constructor for EF Core
    private ServiceInquiry() { }

    public ServiceInquiry(
        Guid tenantId,
        Guid serviceId,
        string name,
        string email,
        string? company,
        string projectDescription)
    {
        ServiceInquiryId = Guid.NewGuid();
        TenantId = tenantId;
        ServiceId = serviceId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Company = company;
        ProjectDescription = projectDescription ?? throw new ArgumentNullException(nameof(projectDescription));
        CreatedAt = DateTime.UtcNow;
    }
}
