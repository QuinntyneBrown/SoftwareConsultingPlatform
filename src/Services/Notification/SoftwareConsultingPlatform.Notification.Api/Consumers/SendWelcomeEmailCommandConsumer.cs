using MassTransit;
using SoftwareConsultingPlatform.Notification.Api.Services;
using Shared.Messages.Commands;

namespace SoftwareConsultingPlatform.Notification.Api.Consumers;

public class SendWelcomeEmailCommandConsumer : IConsumer<SendWelcomeEmailCommand>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendWelcomeEmailCommandConsumer> _logger;

    public SendWelcomeEmailCommandConsumer(IEmailService emailService, ILogger<SendWelcomeEmailCommandConsumer> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendWelcomeEmailCommand> context)
    {
        var message = context.Message;
        _logger.LogInformation("Processing SendWelcomeEmailCommand for tenant {TenantId} to {Email}",
            message.TenantId, message.Email);

        var templateData = new Dictionary<string, string>
        {
            ["FullName"] = message.FullName,
            ["VerificationUrl"] = message.VerificationUrl,
            ["CompanyName"] = "Software Consulting Platform"
        };

        try
        {
            await _emailService.SendTemplatedAsync("welcome", message.Email, templateData, context.CancellationToken);
            _logger.LogInformation("Welcome email sent successfully to {Email}", message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send welcome email to {Email}", message.Email);
            throw;
        }
    }
}
