namespace Shared.Messages.Events.Services;

public record ServicePublishedEvent(
    Guid ServiceId,
    Guid TenantId,
    string Name,
    string Slug,
    DateTime PublishedAt
);
