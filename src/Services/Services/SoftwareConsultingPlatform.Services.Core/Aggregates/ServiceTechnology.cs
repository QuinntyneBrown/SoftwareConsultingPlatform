namespace SoftwareConsultingPlatform.Services.Core.Aggregates;

public class ServiceTechnology
{
    public Guid ServiceId { get; private set; }
    public Guid TechnologyId { get; private set; }

    public Service? Service { get; private set; }
    public Technology? Technology { get; private set; }

    private ServiceTechnology() { }

    public ServiceTechnology(Guid serviceId, Guid technologyId)
    {
        ServiceId = serviceId;
        TechnologyId = technologyId;
    }
}
