namespace SoftwareConsultingPlatform.Core.Models.ServiceAggregate;

/// <summary>
/// Many-to-many relationship between Service and Technology.
/// </summary>
public class ServiceTechnology
{
    public Guid ServiceId { get; set; }
    public Guid TechnologyId { get; set; }

    // Navigation properties
    public Service Service { get; set; } = null!;
}
