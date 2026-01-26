namespace SoftwareConsultingPlatform.Notification.Api.Services;

public interface ITemplateService
{
    Task<(string Subject, string Body)> RenderAsync(string templateName, Dictionary<string, string> data);
}
