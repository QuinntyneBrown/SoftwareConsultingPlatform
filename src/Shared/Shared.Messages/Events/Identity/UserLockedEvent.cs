namespace Shared.Messages.Events.Identity;

public record UserLockedEvent(
    Guid UserId,
    Guid TenantId,
    DateTime LockedUntil,
    string Reason
);
