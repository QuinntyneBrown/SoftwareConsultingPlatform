namespace SoftwareConsultingPlatform.Services.Core.Aggregates;

public class ServiceFaq
{
    public Guid ServiceFaqId { get; private set; }
    public Guid ServiceId { get; private set; }
    public string Question { get; private set; } = string.Empty;
    public string Answer { get; private set; } = string.Empty;
    public int DisplayOrder { get; private set; }

    public Service? Service { get; private set; }

    private ServiceFaq() { }

    public ServiceFaq(Guid serviceId, string question, string answer, int displayOrder = 0)
    {
        ServiceFaqId = Guid.NewGuid();
        ServiceId = serviceId;
        Question = question ?? throw new ArgumentNullException(nameof(question));
        Answer = answer ?? throw new ArgumentNullException(nameof(answer));
        DisplayOrder = displayOrder;
    }

    public void Update(string question, string answer, int displayOrder)
    {
        Question = question;
        Answer = answer;
        DisplayOrder = displayOrder;
    }
}
