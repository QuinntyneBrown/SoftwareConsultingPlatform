using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace SoftwareConsultingPlatform.Notification.Api.Services;

public class SmtpEmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ITemplateService _templateService;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IOptions<EmailSettings> settings, ITemplateService templateService, ILogger<SmtpEmailService> logger)
    {
        _settings = settings.Value;
        _templateService = templateService;
        _logger = logger;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken ct = default)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_settings.FromName, message.From ?? _settings.FromEmail));
        email.To.Add(MailboxAddress.Parse(message.To));

        if (message.Cc != null)
        {
            foreach (var cc in message.Cc)
                email.Cc.Add(MailboxAddress.Parse(cc));
        }

        if (message.Bcc != null)
        {
            foreach (var bcc in message.Bcc)
                email.Bcc.Add(MailboxAddress.Parse(bcc));
        }

        email.Subject = message.Subject;

        var builder = new BodyBuilder();
        if (message.IsHtml)
            builder.HtmlBody = message.Body;
        else
            builder.TextBody = message.Body;

        if (message.Attachments != null)
        {
            foreach (var attachment in message.Attachments)
            {
                builder.Attachments.Add(attachment.FileName, attachment.Content, ContentType.Parse(attachment.ContentType));
            }
        }

        email.Body = builder.ToMessageBody();

        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, _settings.UseSsl, ct);

            if (!string.IsNullOrEmpty(_settings.SmtpUsername))
            {
                await client.AuthenticateAsync(_settings.SmtpUsername, _settings.SmtpPassword, ct);
            }

            await client.SendAsync(email, ct);
            await client.DisconnectAsync(true, ct);

            _logger.LogInformation("Email sent successfully to {To}", message.To);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", message.To);
            throw;
        }
    }

    public async Task SendTemplatedAsync(string templateName, string to, Dictionary<string, string> data, CancellationToken ct = default)
    {
        var (subject, body) = await _templateService.RenderAsync(templateName, data);
        await SendAsync(new EmailMessage(to, subject, body), ct);
    }
}
