namespace Shared.Messages.Events.Services;

public record ServiceCreatedEvent(
    Guid ServiceId,
    Guid TenantId,
    string Name,
    string Slug,
    DateTime CreatedAt
);
