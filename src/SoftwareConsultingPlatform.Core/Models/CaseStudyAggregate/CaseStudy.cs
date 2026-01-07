using SoftwareConsultingPlatform.Core.Models.CaseStudyAggregate.Enums;

namespace SoftwareConsultingPlatform.Core.Models.CaseStudyAggregate;

/// <summary>
/// Case study aggregate root representing a project showcase.
/// </summary>
public class CaseStudy
{
    public Guid CaseStudyId { get; private set; }
    public Guid TenantId { get; private set; }
    public string ClientName { get; private set; } = string.Empty;
    public string ProjectTitle { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string Overview { get; private set; } = string.Empty;
    public string Challenge { get; private set; } = string.Empty;
    public string Solution { get; private set; } = string.Empty;
    public string Results { get; private set; } = string.Empty;

    // Metadata
    public CaseStudyStatus Status { get; private set; }
    public bool Featured { get; private set; }
    public int FeaturedOrder { get; private set; }
    public DateTime? PublishedDate { get; private set; }

    // Testimonial
    public string? TestimonialQuote { get; private set; }
    public string? TestimonialAuthor { get; private set; }
    public string? TestimonialAuthorRole { get; private set; }
    public string? TestimonialAuthorCompany { get; private set; }

    // SEO
    public string? MetaTitle { get; private set; }
    public string? MetaDescription { get; private set; }
    public string? CanonicalUrl { get; private set; }

    // Metrics (stored as JSON)
    public string MetricsJson { get; private set; } = "[]";

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Private constructor for EF Core
    private CaseStudy() { }

    public CaseStudy(
        Guid tenantId,
        string clientName,
        string projectTitle,
        string slug,
        string overview,
        string challenge,
        string solution,
        string results)
    {
        CaseStudyId = Guid.NewGuid();
        TenantId = tenantId;
        ClientName = clientName ?? throw new ArgumentNullException(nameof(clientName));
        ProjectTitle = projectTitle ?? throw new ArgumentNullException(nameof(projectTitle));
        Slug = slug ?? throw new ArgumentNullException(nameof(slug));
        Overview = overview ?? throw new ArgumentNullException(nameof(overview));
        Challenge = challenge ?? throw new ArgumentNullException(nameof(challenge));
        Solution = solution ?? throw new ArgumentNullException(nameof(solution));
        Results = results ?? throw new ArgumentNullException(nameof(results));
        Status = CaseStudyStatus.Draft;
        Featured = false;
        FeaturedOrder = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        string? clientName,
        string? projectTitle,
        string? overview,
        string? challenge,
        string? solution,
        string? results)
    {
        if (!string.IsNullOrWhiteSpace(clientName))
            ClientName = clientName;
        if (!string.IsNullOrWhiteSpace(projectTitle))
            ProjectTitle = projectTitle;
        if (!string.IsNullOrWhiteSpace(overview))
            Overview = overview;
        if (!string.IsNullOrWhiteSpace(challenge))
            Challenge = challenge;
        if (!string.IsNullOrWhiteSpace(solution))
            Solution = solution;
        if (!string.IsNullOrWhiteSpace(results))
            Results = results;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Publish()
    {
        Status = CaseStudyStatus.Published;
        PublishedDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Archive()
    {
        Status = CaseStudyStatus.Archived;
        Featured = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetFeatured(bool featured, int order = 0)
    {
        Featured = featured;
        FeaturedOrder = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTestimonial(
        string? quote,
        string? author,
        string? authorRole,
        string? authorCompany)
    {
        TestimonialQuote = quote;
        TestimonialAuthor = author;
        TestimonialAuthorRole = authorRole;
        TestimonialAuthorCompany = authorCompany;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateSeo(string? metaTitle, string? metaDescription, string? canonicalUrl)
    {
        MetaTitle = metaTitle;
        MetaDescription = metaDescription;
        CanonicalUrl = canonicalUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateMetrics(string metricsJson)
    {
        MetricsJson = metricsJson ?? "[]";
        UpdatedAt = DateTime.UtcNow;
    }
}
