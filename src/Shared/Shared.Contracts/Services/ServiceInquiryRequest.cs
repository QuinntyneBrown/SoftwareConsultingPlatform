namespace Shared.Contracts.Services;

public record ServiceInquiryRequest(
    string Name,
    string Email,
    string? Company,
    string ProjectDescription
);
