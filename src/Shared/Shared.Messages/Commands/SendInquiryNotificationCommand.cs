namespace Shared.Messages.Commands;

public record SendInquiryNotificationCommand(
    Guid TenantId,
    Guid ServiceId,
    string ServiceName,
    string InquirerName,
    string InquirerEmail,
    string ProjectDescription,
    string? Company = null
);
