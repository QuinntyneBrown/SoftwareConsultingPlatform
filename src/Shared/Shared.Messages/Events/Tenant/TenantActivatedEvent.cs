namespace Shared.Messages.Events.Tenant;

public record TenantActivatedEvent(
    Guid TenantId,
    DateTime ActivatedAt
);
