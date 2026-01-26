namespace SoftwareConsultingPlatform.CaseStudies.Core.Aggregates;

public class CaseStudyTechnology
{
    public Guid CaseStudyId { get; private set; }
    public Guid TechnologyId { get; private set; }
    public CaseStudy? CaseStudy { get; private set; }
    public Technology? Technology { get; private set; }
    private CaseStudyTechnology() { }
    public CaseStudyTechnology(Guid caseStudyId, Guid technologyId) { CaseStudyId = caseStudyId; TechnologyId = technologyId; }
}
