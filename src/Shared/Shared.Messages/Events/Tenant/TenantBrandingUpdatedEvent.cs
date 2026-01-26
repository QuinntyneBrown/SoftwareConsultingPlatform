namespace Shared.Messages.Events.Tenant;

public record TenantBrandingUpdatedEvent(
    Guid TenantId,
    string? LogoUrl,
    string? PrimaryColor,
    string? SecondaryColor
);
