namespace SoftwareConsultingPlatform.Core.Models.CaseStudyAggregate;

/// <summary>
/// Image associated with a case study.
/// </summary>
public class CaseStudyImage
{
    public Guid CaseStudyImageId { get; set; }
    public Guid CaseStudyId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? Caption { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public CaseStudy CaseStudy { get; set; } = null!;
}
