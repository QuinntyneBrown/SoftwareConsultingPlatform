namespace SoftwareConsultingPlatform.Core.Models.ServiceAggregate;

/// <summary>
/// FAQ entry for a service.
/// </summary>
public class ServiceFaq
{
    public Guid ServiceFaqId { get; set; }
    public Guid ServiceId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public Service Service { get; set; } = null!;
}
