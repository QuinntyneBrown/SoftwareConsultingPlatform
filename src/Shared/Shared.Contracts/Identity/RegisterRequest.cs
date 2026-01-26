namespace Shared.Contracts.Identity;

public record RegisterRequest(
    string Email,
    string Password,
    string FullName,
    string? Phone,
    string? CompanyName,
    Guid TenantId
);
