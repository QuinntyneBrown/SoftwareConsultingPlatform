using SoftwareConsultingPlatform.Tenant.Core.ValueObjects;

namespace SoftwareConsultingPlatform.Tenant.Core.Aggregates;

public class Tenant
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Subdomain { get; private set; } = string.Empty;
    public string? CustomDomain { get; private set; }
    public TenantStatus Status { get; private set; }

    // Branding
    public string? LogoUrl { get; private set; }
    public string? FaviconUrl { get; private set; }
    public string? PrimaryColor { get; private set; }
    public string? SecondaryColor { get; private set; }
    public string? FontFamily { get; private set; }

    // Contact Info
    public string? ContactEmail { get; private set; }
    public string? ContactPhone { get; private set; }
    public string? Address { get; private set; }
    public string? SupportEmail { get; private set; }

    // Billing
    public string? Plan { get; private set; }
    public string? BillingEmail { get; private set; }
    public string? SubscriptionStatus { get; private set; }

    // Configuration
    public string? FeaturesJson { get; private set; }
    public string? SettingsJson { get; private set; }
    public string? MetadataJson { get; private set; }

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? ActivatedAt { get; private set; }

    private Tenant() { }

    public Tenant(string name, string subdomain)
    {
        TenantId = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Subdomain = subdomain ?? throw new ArgumentNullException(nameof(subdomain));
        Status = TenantStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateBranding(string? logoUrl, string? faviconUrl, string? primaryColor, string? secondaryColor, string? fontFamily)
    {
        LogoUrl = logoUrl;
        FaviconUrl = faviconUrl;
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
        FontFamily = fontFamily;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateContactInfo(string? email, string? phone, string? address, string? supportEmail)
    {
        ContactEmail = email;
        ContactPhone = phone;
        Address = address;
        SupportEmail = supportEmail;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCustomDomain(string? customDomain)
    {
        CustomDomain = customDomain;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = TenantStatus.Active;
        ActivatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Suspend()
    {
        Status = TenantStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = TenantStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateConfiguration(string? featuresJson, string? settingsJson, string? metadataJson)
    {
        FeaturesJson = featuresJson;
        SettingsJson = settingsJson;
        MetadataJson = metadataJson;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePlan(string? plan, string? billingEmail)
    {
        Plan = plan;
        BillingEmail = billingEmail;
        UpdatedAt = DateTime.UtcNow;
    }
}
