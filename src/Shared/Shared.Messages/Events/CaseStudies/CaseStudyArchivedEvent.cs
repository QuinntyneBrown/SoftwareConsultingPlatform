namespace Shared.Messages.Events.CaseStudies;

public record CaseStudyArchivedEvent(
    Guid CaseStudyId,
    Guid TenantId,
    DateTime ArchivedAt
);
