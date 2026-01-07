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
}
