namespace Shared.Messages.Events.CaseStudies;

public record CaseStudyCreatedEvent(
    Guid CaseStudyId,
    Guid TenantId,
    string ClientName,
    string ProjectTitle,
    string Slug,
    DateTime CreatedAt
);
