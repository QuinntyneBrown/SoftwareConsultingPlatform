namespace Shared.Messages.Events.Services;

public record ServiceFeaturedChangedEvent(
    Guid ServiceId,
    Guid TenantId,
    bool IsFeatured,
    int? DisplayOrder
);
