namespace Shared.Contracts.Services;

public record ServiceSummaryDto(
    Guid ServiceId,
    string Name,
    string Slug,
    string? Tagline,
    string? IconUrl,
    bool Featured,
    int DisplayOrder
);
