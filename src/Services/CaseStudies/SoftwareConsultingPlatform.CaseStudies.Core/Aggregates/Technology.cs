namespace SoftwareConsultingPlatform.CaseStudies.Core.Aggregates;

public class Technology
{
    public Guid TechnologyId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Icon { get; private set; }
    public string? Category { get; private set; }
    public ICollection<CaseStudyTechnology> CaseStudies { get; private set; } = new List<CaseStudyTechnology>();
    private Technology() { }
    public Technology(string name, string? icon = null, string? category = null)
    {
        TechnologyId = Guid.NewGuid();
        Name = name;
        Icon = icon;
        Category = category;
    }
}
