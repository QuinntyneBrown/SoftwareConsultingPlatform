namespace SoftwareConsultingPlatform.Core.Models.HomepageAggregate;

/// <summary>
/// Homepage content aggregate for managing homepage sections.
/// </summary>
public class HomepageContent
{
    public Guid HomepageContentId { get; private set; }
    public Guid TenantId { get; private set; }

    // Hero Section
    public string HeroTitle { get; private set; } = string.Empty;
    public string HeroSubtitle { get; private set; } = string.Empty;
    public string? HeroImageUrl { get; private set; }
    public string? HeroCtaText { get; private set; }
    public string? HeroCtaUrl { get; private set; }

    // Services Overview (stored as JSON)
    public string ServicesJson { get; private set; } = "[]";

    // Testimonials (stored as JSON)
    public string TestimonialsJson { get; private set; } = "[]";

    // Additional Sections (stored as JSON)
    public string SectionsJson { get; private set; } = "[]";

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }

    // Private constructor for EF Core
    private HomepageContent() { }

    public HomepageContent(
        Guid tenantId,
        string heroTitle,
        string heroSubtitle)
    {
        HomepageContentId = Guid.NewGuid();
        TenantId = tenantId;
        HeroTitle = heroTitle ?? throw new ArgumentNullException(nameof(heroTitle));
        HeroSubtitle = heroSubtitle ?? throw new ArgumentNullException(nameof(heroSubtitle));
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateHeroSection(
        string heroTitle,
        string heroSubtitle,
        string? heroImageUrl,
        string? heroCtaText,
        string? heroCtaUrl)
    {
        HeroTitle = heroTitle ?? throw new ArgumentNullException(nameof(heroTitle));
        HeroSubtitle = heroSubtitle ?? throw new ArgumentNullException(nameof(heroSubtitle));
        HeroImageUrl = heroImageUrl;
        HeroCtaText = heroCtaText;
        HeroCtaUrl = heroCtaUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateServices(string servicesJson)
    {
        ServicesJson = servicesJson ?? "[]";
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTestimonials(string testimonialsJson)
    {
        TestimonialsJson = testimonialsJson ?? "[]";
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateSections(string sectionsJson)
    {
        SectionsJson = sectionsJson ?? "[]";
        UpdatedAt = DateTime.UtcNow;
    }
}
