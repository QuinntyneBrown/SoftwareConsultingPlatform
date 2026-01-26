namespace Shared.Messages.Commands;

public record SendWelcomeEmailCommand(
    Guid TenantId,
    string Email,
    string FullName,
    string VerificationUrl
);
