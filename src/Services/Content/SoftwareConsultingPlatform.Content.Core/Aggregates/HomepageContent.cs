namespace SoftwareConsultingPlatform.Content.Core.Aggregates;

public class HomepageContent
{
    public Guid HomepageContentId { get; private set; }
    public Guid TenantId { get; private set; }
    public string? HeroTitle { get; private set; }
    public string? HeroSubtitle { get; private set; }
    public string? HeroImageUrl { get; private set; }
    public string? HeroCtaText { get; private set; }
    public string? HeroCtaUrl { get; private set; }
    public string? ServicesJson { get; private set; }
    public string? TestimonialsJson { get; private set; }
    public string? SectionsJson { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }

    private HomepageContent() { }

    public HomepageContent(Guid tenantId)
    {
        HomepageContentId = Guid.NewGuid();
        TenantId = tenantId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateHero(string? title, string? subtitle, string? imageUrl, string? ctaText, string? ctaUrl, string? updatedBy)
    {
        HeroTitle = title; HeroSubtitle = subtitle; HeroImageUrl = imageUrl; HeroCtaText = ctaText; HeroCtaUrl = ctaUrl;
        UpdatedBy = updatedBy; UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateServices(string? servicesJson, string? updatedBy) { ServicesJson = servicesJson; UpdatedBy = updatedBy; UpdatedAt = DateTime.UtcNow; }
    public void UpdateTestimonials(string? json, string? updatedBy) { TestimonialsJson = json; UpdatedBy = updatedBy; UpdatedAt = DateTime.UtcNow; }
    public void UpdateSections(string? json, string? updatedBy) { SectionsJson = json; UpdatedBy = updatedBy; UpdatedAt = DateTime.UtcNow; }
}
