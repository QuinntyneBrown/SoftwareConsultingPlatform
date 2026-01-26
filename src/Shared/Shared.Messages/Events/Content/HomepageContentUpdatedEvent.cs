namespace Shared.Messages.Events.Content;

public record HomepageContentUpdatedEvent(
    Guid ContentId,
    Guid TenantId,
    string UpdatedBy,
    DateTime UpdatedAt
);
