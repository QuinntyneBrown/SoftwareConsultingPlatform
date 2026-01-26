namespace SoftwareConsultingPlatform.Notification.Api.Services;

public interface IEmailService
{
    Task SendAsync(EmailMessage message, CancellationToken ct = default);
    Task SendTemplatedAsync(string templateName, string to, Dictionary<string, string> data, CancellationToken ct = default);
}

public record EmailMessage(
    string To,
    string Subject,
    string Body,
    bool IsHtml = true,
    string? From = null,
    List<string>? Cc = null,
    List<string>? Bcc = null,
    List<EmailAttachment>? Attachments = null
);

public record EmailAttachment(
    string FileName,
    byte[] Content,
    string ContentType
);

public class EmailSettings
{
    public string SmtpHost { get; set; } = "localhost";
    public int SmtpPort { get; set; } = 587;
    public string? SmtpUsername { get; set; }
    public string? SmtpPassword { get; set; }
    public bool UseSsl { get; set; } = true;
    public string FromEmail { get; set; } = "noreply@example.com";
    public string FromName { get; set; } = "Software Consulting Platform";
    public string? SendGridApiKey { get; set; }
    public EmailProvider Provider { get; set; } = EmailProvider.Smtp;
}

public enum EmailProvider
{
    Smtp,
    SendGrid
}
