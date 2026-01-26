namespace SoftwareConsultingPlatform.CaseStudies.Core.Aggregates;

public class CaseStudyImage
{
    public Guid CaseStudyImageId { get; private set; }
    public Guid CaseStudyId { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public string? ThumbnailUrl { get; private set; }
    public string? Caption { get; private set; }
    public int DisplayOrder { get; private set; }
    public CaseStudy? CaseStudy { get; private set; }
    private CaseStudyImage() { }
    public CaseStudyImage(Guid caseStudyId, string imageUrl, string? thumbnailUrl, string? caption, int displayOrder)
    {
        CaseStudyImageId = Guid.NewGuid();
        CaseStudyId = caseStudyId;
        ImageUrl = imageUrl;
        ThumbnailUrl = thumbnailUrl;
        Caption = caption;
        DisplayOrder = displayOrder;
    }
}
