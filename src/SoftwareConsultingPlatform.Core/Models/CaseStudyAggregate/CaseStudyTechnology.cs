namespace SoftwareConsultingPlatform.Core.Models.CaseStudyAggregate;

/// <summary>
/// Many-to-many relationship between CaseStudy and Technology.
/// </summary>
public class CaseStudyTechnology
{
    public Guid CaseStudyId { get; set; }
    public Guid TechnologyId { get; set; }

    // Navigation properties
    public CaseStudy CaseStudy { get; set; } = null!;
    public Technology Technology { get; set; } = null!;
}
