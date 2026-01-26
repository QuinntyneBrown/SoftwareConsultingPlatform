namespace Shared.Contracts.Content;

public record HomepageContentDto(
    Guid ContentId,
    HeroSectionDto Hero,
    List<HomepageServiceDto> Services,
    List<HomepageTestimonialDto> Testimonials,
    List<HomepageSectionDto> Sections
);

public record HeroSectionDto(
    string? Title,
    string? Subtitle,
    string? ImageUrl,
    string? CtaText,
    string? CtaUrl
);

public record HomepageServiceDto(
    string Name,
    string? Description,
    string? IconUrl,
    string? Url
);

public record HomepageTestimonialDto(
    string Quote,
    string? Author,
    string? AuthorRole,
    string? AuthorCompany,
    string? AuthorImageUrl
);

public record HomepageSectionDto(
    string Title,
    string? Content,
    string? ImageUrl,
    int DisplayOrder
);
