namespace Shared.Messages.Events.Content;

public record FeaturedItemRemovedEvent(
    Guid TenantId,
    string ItemType,
    Guid ItemId
);
