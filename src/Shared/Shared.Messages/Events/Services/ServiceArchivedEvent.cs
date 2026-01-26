namespace Shared.Messages.Events.Services;

public record ServiceArchivedEvent(
    Guid ServiceId,
    Guid TenantId,
    DateTime ArchivedAt
);
