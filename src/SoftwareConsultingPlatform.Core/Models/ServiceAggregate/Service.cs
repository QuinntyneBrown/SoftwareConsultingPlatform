using SoftwareConsultingPlatform.Core.Models.ServiceAggregate.Enums;

namespace SoftwareConsultingPlatform.Core.Models.ServiceAggregate;

/// <summary>
/// Service aggregate root representing a consulting service offering.
/// </summary>
public class Service
{
    public Guid ServiceId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string Tagline { get; private set; } = string.Empty;
    public string Overview { get; private set; } = string.Empty;
    public string? IconUrl { get; private set; }

    // Details (stored as JSON arrays)
    public string WhatWeDoJson { get; private set; } = "[]";
    public string HowWeWorkJson { get; private set; } = "[]";
    public string BenefitsJson { get; private set; } = "[]";
    public string PricingModelsJson { get; private set; } = "[]";
    public string EngagementTypesJson { get; private set; } = "[]";

    // Metadata
    public ServiceStatus Status { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool Featured { get; private set; }

    // SEO
    public string? MetaTitle { get; private set; }
    public string? MetaDescription { get; private set; }
    public string? CanonicalUrl { get; private set; }

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? PublishedAt { get; private set; }

    // Private constructor for EF Core
    private Service() { }

    public Service(
        Guid tenantId,
        string name,
        string slug,
        string tagline,
        string overview)
    {
        ServiceId = Guid.NewGuid();
        TenantId = tenantId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Slug = slug ?? throw new ArgumentNullException(nameof(slug));
        Tagline = tagline ?? throw new ArgumentNullException(nameof(tagline));
        Overview = overview ?? throw new ArgumentNullException(nameof(overview));
        Status = ServiceStatus.Draft;
        DisplayOrder = 0;
        Featured = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        string? name,
        string? tagline,
        string? overview,
        string? iconUrl)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;
        if (!string.IsNullOrWhiteSpace(tagline))
            Tagline = tagline;
        if (!string.IsNullOrWhiteSpace(overview))
            Overview = overview;
        IconUrl = iconUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Publish()
    {
        Status = ServiceStatus.Published;
        PublishedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Archive()
    {
        Status = ServiceStatus.Archived;
        Featured = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDisplayOrder(int order)
    {
        DisplayOrder = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetFeatured(bool featured)
    {
        Featured = featured;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string whatWeDoJson,
        string howWeWorkJson,
        string benefitsJson,
        string pricingModelsJson,
        string engagementTypesJson)
    {
        WhatWeDoJson = whatWeDoJson ?? "[]";
        HowWeWorkJson = howWeWorkJson ?? "[]";
        BenefitsJson = benefitsJson ?? "[]";
        PricingModelsJson = pricingModelsJson ?? "[]";
        EngagementTypesJson = engagementTypesJson ?? "[]";
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
