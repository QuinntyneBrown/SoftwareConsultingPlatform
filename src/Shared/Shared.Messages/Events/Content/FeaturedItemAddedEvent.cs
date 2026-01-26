namespace Shared.Messages.Events.Content;

public record FeaturedItemAddedEvent(
    Guid TenantId,
    string ItemType,
    Guid ItemId,
    int DisplayOrder
);
