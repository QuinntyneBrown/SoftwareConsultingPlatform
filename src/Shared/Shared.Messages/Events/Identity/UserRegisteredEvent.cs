namespace Shared.Messages.Events.Identity;

public record UserRegisteredEvent(
    Guid UserId,
    Guid TenantId,
    string Email,
    string FullName,
    DateTime RegisteredAt
);
