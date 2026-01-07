namespace SoftwareConsultingPlatform.Api.Features.Homepage.GetFeaturedCaseStudies;

public sealed class GetFeaturedCaseStudiesResponseItem
{
    public Guid CaseStudyId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ProjectTitle { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
}