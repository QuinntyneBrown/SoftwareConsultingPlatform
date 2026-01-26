namespace Shared.Messages.Events.Identity;

public record UserEmailVerifiedEvent(
    Guid UserId,
    Guid TenantId,
    DateTime VerifiedAt
);
