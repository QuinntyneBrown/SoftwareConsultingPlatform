namespace Shared.Contracts.CaseStudies;

public record CreateCaseStudyRequest(
    string ClientName,
    string ProjectTitle,
    string? Overview,
    string? Challenge,
    string? Solution,
    string? Results
);
