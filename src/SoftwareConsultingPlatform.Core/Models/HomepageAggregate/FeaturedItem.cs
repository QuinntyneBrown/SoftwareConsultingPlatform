namespace SoftwareConsultingPlatform.Core.Models.HomepageAggregate;

/// <summary>
/// Featured items for homepage display (case studies, services, etc.).
/// </summary>
public class FeaturedItem
{
    public Guid FeaturedItemId { get; set; }
    public Guid TenantId { get; set; }
    public string ItemType { get; set; } = string.Empty; // "CaseStudy", "Service"
    public Guid ItemId { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
}
