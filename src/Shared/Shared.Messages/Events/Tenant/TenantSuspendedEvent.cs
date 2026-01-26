namespace Shared.Messages.Events.Tenant;

public record TenantSuspendedEvent(
    Guid TenantId,
    string Reason,
    DateTime SuspendedAt
);
