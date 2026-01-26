using MassTransit;
using SoftwareConsultingPlatform.Notification.Api.Services;
using Shared.Messages.Commands;

namespace SoftwareConsultingPlatform.Notification.Api.Consumers;

public class SendPasswordResetEmailCommandConsumer : IConsumer<SendPasswordResetEmailCommand>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendPasswordResetEmailCommandConsumer> _logger;

    public SendPasswordResetEmailCommandConsumer(IEmailService emailService, ILogger<SendPasswordResetEmailCommandConsumer> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendPasswordResetEmailCommand> context)
    {
        var message = context.Message;
        _logger.LogInformation("Processing SendPasswordResetEmailCommand for tenant {TenantId} to {Email}",
            message.TenantId, message.Email);

        var templateData = new Dictionary<string, string>
        {
            ["ResetUrl"] = message.ResetUrl,
            ["ExpiresAt"] = message.ExpiresAt.ToString("f"),
            ["CompanyName"] = "Software Consulting Platform"
        };

        try
        {
            await _emailService.SendTemplatedAsync("password-reset", message.Email, templateData, context.CancellationToken);
            _logger.LogInformation("Password reset email sent successfully to {Email}", message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", message.Email);
            throw;
        }
    }
}
