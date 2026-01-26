using SoftwareConsultingPlatform.CaseStudies.Core.ValueObjects;

namespace SoftwareConsultingPlatform.CaseStudies.Core.Aggregates;

public class CaseStudy
{
    public Guid CaseStudyId { get; private set; }
    public Guid TenantId { get; private set; }
    public string ClientName { get; private set; } = string.Empty;
    public string ProjectTitle { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? Overview { get; private set; }
    public string? Challenge { get; private set; }
    public string? Solution { get; private set; }
    public string? Results { get; private set; }
    public string? TestimonialQuote { get; private set; }
    public string? TestimonialAuthor { get; private set; }
    public string? TestimonialAuthorRole { get; private set; }
    public string? TestimonialAuthorCompany { get; private set; }
    public CaseStudyStatus Status { get; private set; }
    public bool Featured { get; private set; }
    public int? FeaturedOrder { get; private set; }
    public string? MetaTitle { get; private set; }
    public string? MetaDescription { get; private set; }
    public string? CanonicalUrl { get; private set; }
    public string? MetricsJson { get; private set; }
    public DateTime? PublishedDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public ICollection<CaseStudyTechnology> Technologies { get; private set; } = new List<CaseStudyTechnology>();
    public ICollection<CaseStudyImage> Images { get; private set; } = new List<CaseStudyImage>();

    private CaseStudy() { }

    public CaseStudy(Guid tenantId, string clientName, string projectTitle, string slug)
    {
        CaseStudyId = Guid.NewGuid();
        TenantId = tenantId;
        ClientName = clientName;
        ProjectTitle = projectTitle;
        Slug = slug;
        Status = CaseStudyStatus.Draft;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string? overview, string? challenge, string? solution, string? results)
    {
        Overview = overview;
        Challenge = challenge;
        Solution = solution;
        Results = results;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTestimonial(string? quote, string? author, string? role, string? company)
    {
        TestimonialQuote = quote;
        TestimonialAuthor = author;
        TestimonialAuthorRole = role;
        TestimonialAuthorCompany = company;
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
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetFeatured(bool featured, int? order = null)
    {
        Featured = featured;
        FeaturedOrder = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateSeo(string? metaTitle, string? metaDescription, string? canonicalUrl)
    {
        MetaTitle = metaTitle;
        MetaDescription = metaDescription;
        CanonicalUrl = canonicalUrl;
        UpdatedAt = DateTime.UtcNow;
    }
}
