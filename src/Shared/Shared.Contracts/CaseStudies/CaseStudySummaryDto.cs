namespace Shared.Contracts.CaseStudies;

public record CaseStudySummaryDto(
    Guid CaseStudyId,
    string ClientName,
    string ProjectTitle,
    string Slug,
    string? Overview,
    string? ThumbnailUrl,
    bool Featured,
    int? FeaturedOrder
);
