namespace Shared.Messages.Events.Services;

public record ServiceInquirySubmittedEvent(
    Guid InquiryId,
    Guid ServiceId,
    Guid TenantId,
    string Name,
    string Email,
    string? Company,
    string ProjectDescription,
    DateTime SubmittedAt
);
