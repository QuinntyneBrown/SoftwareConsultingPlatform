namespace Shared.Contracts.Content;

public record UpdateHomepageContentRequest(
    HeroSectionDto? Hero,
    List<HomepageServiceDto>? Services,
    List<HomepageTestimonialDto>? Testimonials,
    List<HomepageSectionDto>? Sections
);
