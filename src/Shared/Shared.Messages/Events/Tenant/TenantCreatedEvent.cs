namespace Shared.Messages.Events.Tenant;

public record TenantCreatedEvent(
    Guid TenantId,
    string Name,
    string Subdomain,
    DateTime CreatedAt
);
