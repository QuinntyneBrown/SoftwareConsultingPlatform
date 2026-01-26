namespace Shared.Messages.Commands;

public record SendPasswordResetEmailCommand(
    Guid TenantId,
    string Email,
    string ResetUrl,
    DateTime ExpiresAt
);
