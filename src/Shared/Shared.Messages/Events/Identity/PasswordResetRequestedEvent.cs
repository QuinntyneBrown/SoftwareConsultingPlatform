namespace Shared.Messages.Events.Identity;

public record PasswordResetRequestedEvent(
    Guid UserId,
    Guid TenantId,
    string Email,
    string ResetToken,
    DateTime ExpiresAt
);
