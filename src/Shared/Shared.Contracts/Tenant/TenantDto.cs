namespace Shared.Contracts.Tenant;

public record TenantDto(
    Guid TenantId,
    string Name,
    string Subdomain,
    string? CustomDomain,
    string Status,
    TenantBrandingDto Branding,
    TenantContactDto Contact
);

public record TenantBrandingDto(
    string? LogoUrl,
    string? FaviconUrl,
    string? PrimaryColor,
    string? SecondaryColor,
    string? FontFamily
);

public record TenantContactDto(
    string? Email,
    string? Phone,
    string? Address,
    string? SupportEmail
);
