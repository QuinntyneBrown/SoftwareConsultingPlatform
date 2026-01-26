namespace Shared.Contracts.Tenant;

public record UpdateTenantRequest(
    string? Name,
    string? CustomDomain,
    TenantBrandingDto? Branding,
    TenantContactDto? Contact
);
