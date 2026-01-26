namespace Shared.Messages.Events.CaseStudies;

public record CaseStudyFeaturedChangedEvent(
    Guid CaseStudyId,
    Guid TenantId,
    bool IsFeatured,
    int? FeaturedOrder
);
