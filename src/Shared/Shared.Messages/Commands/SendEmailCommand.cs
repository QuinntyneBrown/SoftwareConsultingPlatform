namespace Shared.Messages.Commands;

public record SendEmailCommand(
    Guid TenantId,
    string To,
    string Subject,
    string TemplateName,
    Dictionary<string, string> TemplateData
);
