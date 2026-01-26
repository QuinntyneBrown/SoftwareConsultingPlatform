using MassTransit;
using SoftwareConsultingPlatform.Notification.Api.Services;
using Shared.Messages.Commands;

namespace SoftwareConsultingPlatform.Notification.Api.Consumers;

public class SendEmailCommandConsumer : IConsumer<SendEmailCommand>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailCommandConsumer> _logger;

    public SendEmailCommandConsumer(IEmailService emailService, ILogger<SendEmailCommandConsumer> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendEmailCommand> context)
    {
        var message = context.Message;
        _logger.LogInformation("Processing SendEmailCommand for tenant {TenantId} to {To} with template {Template}",
            message.TenantId, message.To, message.TemplateName);

        try
        {
            await _emailService.SendTemplatedAsync(message.TemplateName, message.To, message.TemplateData, context.CancellationToken);
            _logger.LogInformation("Email sent successfully to {To}", message.To);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", message.To);
            throw;
        }
    }
}
