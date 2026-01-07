namespace SoftwareConsultingPlatform.Core.Models.CaseStudyAggregate;

/// <summary>
/// Technology entity shared across case studies and services.
/// </summary>
public class Technology
{
    public Guid TechnologyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? IconUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
