using SoftwareConsultingPlatform.Services.Core.ValueObjects;

namespace SoftwareConsultingPlatform.Services.Core.Aggregates;

public class Service
{
    public Guid ServiceId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? Tagline { get; private set; }
    public string? Overview { get; private set; }
    public string? IconUrl { get; private set; }

    // JSON Content
    public string? WhatWeDoJson { get; private set; }
    public string? HowWeWorkJson { get; private set; }
    public string? BenefitsJson { get; private set; }
    public string? PricingModelsJson { get; private set; }
    public string? EngagementTypesJson { get; private set; }

    // Status
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

    // Navigation
    public ICollection<ServiceTechnology> Technologies { get; private set; } = new List<ServiceTechnology>();
    public ICollection<ServiceFaq> Faqs { get; private set; } = new List<ServiceFaq>();
    public ICollection<ServiceInquiry> Inquiries { get; private set; } = new List<ServiceInquiry>();

    private Service() { }

    public Service(Guid tenantId, string name, string slug)
    {
        ServiceId = Guid.NewGuid();
        TenantId = tenantId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Slug = slug ?? throw new ArgumentNullException(nameof(slug));
        Status = ServiceStatus.Draft;
        DisplayOrder = 0;
        Featured = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string? tagline, string? overview, string? iconUrl)
    {
        Tagline = tagline;
        Overview = overview;
        IconUrl = iconUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateContent(string? whatWeDoJson, string? howWeWorkJson, string? benefitsJson, string? pricingModelsJson, string? engagementTypesJson)
    {
        WhatWeDoJson = whatWeDoJson;
        HowWeWorkJson = howWeWorkJson;
        BenefitsJson = benefitsJson;
        PricingModelsJson = pricingModelsJson;
        EngagementTypesJson = engagementTypesJson;
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
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDisplayOrder(int displayOrder)
    {
        DisplayOrder = displayOrder;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetFeatured(bool featured)
    {
        Featured = featured;
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
