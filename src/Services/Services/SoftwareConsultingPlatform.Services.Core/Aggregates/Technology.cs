namespace SoftwareConsultingPlatform.Services.Core.Aggregates;

public class Technology
{
    public Guid TechnologyId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Icon { get; private set; }
    public string? Category { get; private set; }

    public ICollection<ServiceTechnology> Services { get; private set; } = new List<ServiceTechnology>();

    private Technology() { }

    public Technology(string name, string? icon = null, string? category = null)
    {
        TechnologyId = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Icon = icon;
        Category = category;
    }
}
