namespace Shared.Contracts.Identity;

public record LoginRequest(
    string Email,
    string Password,
    Guid TenantId
);
