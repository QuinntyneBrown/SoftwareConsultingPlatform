namespace Shared.Contracts.Tenant;

public record CreateTenantRequest(
    string Name,
    string Subdomain,
    string? CustomDomain,
    string? ContactEmail,
    string? ContactPhone
);
