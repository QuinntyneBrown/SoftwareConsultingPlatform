using Shared.Contracts.Services;

namespace Shared.Contracts.CaseStudies;

public record CaseStudyDetailDto(
    Guid CaseStudyId,
    string ClientName,
    string ProjectTitle,
    string Slug,
    string? Overview,
    string? Challenge,
    string? Solution,
    string? Results,
    CaseStudyTestimonialDto? Testimonial,
    List<CaseStudyMetricDto> Metrics,
    List<TechnologyDto> Technologies,
    List<CaseStudyImageDto> Images
);

public record CaseStudyTestimonialDto(
    string Quote,
    string? Author,
    string? AuthorRole,
    string? AuthorCompany
);

public record CaseStudyMetricDto(
    string Label,
    string Value
);

public record CaseStudyImageDto(
    string ImageUrl,
    string? ThumbnailUrl,
    string? Caption,
    int DisplayOrder
);
