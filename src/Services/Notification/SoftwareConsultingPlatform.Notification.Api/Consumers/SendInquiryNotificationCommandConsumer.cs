using MassTransit;
using SoftwareConsultingPlatform.Notification.Api.Services;
using Shared.Messages.Commands;

namespace SoftwareConsultingPlatform.Notification.Api.Consumers;

public class SendInquiryNotificationCommandConsumer : IConsumer<SendInquiryNotificationCommand>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendInquiryNotificationCommandConsumer> _logger;

    public SendInquiryNotificationCommandConsumer(IEmailService emailService, ILogger<SendInquiryNotificationCommandConsumer> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendInquiryNotificationCommand> context)
    {
        var message = context.Message;
        _logger.LogInformation("Processing SendInquiryNotificationCommand for service {ServiceId} from {Email}",
            message.ServiceId, message.InquirerEmail);

        // Send notification to tenant admin
        var adminTemplateData = new Dictionary<string, string>
        {
            ["ServiceName"] = message.ServiceName,
            ["InquirerName"] = message.InquirerName,
            ["InquirerEmail"] = message.InquirerEmail,
            ["Company"] = message.Company ?? "Not specified",
            ["ProjectDescription"] = message.ProjectDescription,
            ["CompanyName"] = "Software Consulting Platform"
        };

        // Send confirmation to inquirer
        var confirmationTemplateData = new Dictionary<string, string>
        {
            ["ServiceName"] = message.ServiceName,
            ["InquirerName"] = message.InquirerName,
            ["ProjectDescription"] = message.ProjectDescription,
            ["CompanyName"] = "Software Consulting Platform"
        };

        try
        {
            // Send confirmation to the person who submitted the inquiry
            await _emailService.SendTemplatedAsync("inquiry-confirmation", message.InquirerEmail, confirmationTemplateData, context.CancellationToken);
            _logger.LogInformation("Inquiry confirmation sent to {Email}", message.InquirerEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send inquiry confirmation to {Email}", message.InquirerEmail);
            throw;
        }
    }
}
