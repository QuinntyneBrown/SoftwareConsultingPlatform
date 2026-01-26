namespace Shared.Messages.Events.CaseStudies;

public record CaseStudyPublishedEvent(
    Guid CaseStudyId,
    Guid TenantId,
    string ClientName,
    string ProjectTitle,
    string Slug,
    DateTime PublishedAt
);
