namespace SoftwareConsultingPlatform.Notification.Api.Services;

public class RazorTemplateService : ITemplateService
{
    private readonly ILogger<RazorTemplateService> _logger;
    private readonly Dictionary<string, EmailTemplate> _templates;

    public RazorTemplateService(ILogger<RazorTemplateService> logger)
    {
        _logger = logger;
        _templates = InitializeTemplates();
    }

    public Task<(string Subject, string Body)> RenderAsync(string templateName, Dictionary<string, string> data)
    {
        if (!_templates.TryGetValue(templateName.ToLowerInvariant(), out var template))
        {
            _logger.LogWarning("Template {TemplateName} not found, using default", templateName);
            template = _templates["default"];
        }

        var subject = ReplaceTokens(template.Subject, data);
        var body = ReplaceTokens(template.Body, data);

        return Task.FromResult((subject, body));
    }

    private static string ReplaceTokens(string template, Dictionary<string, string> data)
    {
        foreach (var (key, value) in data)
        {
            template = template.Replace($"{{{{{key}}}}}", value ?? string.Empty);
        }
        return template;
    }

    private static Dictionary<string, EmailTemplate> InitializeTemplates()
    {
        return new Dictionary<string, EmailTemplate>(StringComparer.OrdinalIgnoreCase)
        {
            ["welcome"] = new(
                "Welcome to {{CompanyName}} - Please Verify Your Email",
                """
                <!DOCTYPE html>
                <html>
                <head><meta charset="utf-8"><title>Welcome</title></head>
                <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;">
                    <div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;">
                        <h1 style="color: white; margin: 0;">Welcome to {{CompanyName}}!</h1>
                    </div>
                    <div style="background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px;">
                        <p>Hi {{FullName}},</p>
                        <p>Thank you for joining us! We're excited to have you on board.</p>
                        <p>Please verify your email address by clicking the button below:</p>
                        <div style="text-align: center; margin: 30px 0;">
                            <a href="{{VerificationUrl}}" style="background: #667eea; color: white; padding: 15px 30px; text-decoration: none; border-radius: 5px; display: inline-block;">Verify Email Address</a>
                        </div>
                        <p>If you didn't create an account, you can safely ignore this email.</p>
                        <p>Best regards,<br>The {{CompanyName}} Team</p>
                    </div>
                </body>
                </html>
                """
            ),
            ["password-reset"] = new(
                "Reset Your Password - {{CompanyName}}",
                """
                <!DOCTYPE html>
                <html>
                <head><meta charset="utf-8"><title>Password Reset</title></head>
                <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;">
                    <div style="background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;">
                        <h1 style="color: white; margin: 0;">Password Reset Request</h1>
                    </div>
                    <div style="background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px;">
                        <p>Hi,</p>
                        <p>We received a request to reset your password. Click the button below to create a new password:</p>
                        <div style="text-align: center; margin: 30px 0;">
                            <a href="{{ResetUrl}}" style="background: #f5576c; color: white; padding: 15px 30px; text-decoration: none; border-radius: 5px; display: inline-block;">Reset Password</a>
                        </div>
                        <p style="color: #666; font-size: 14px;">This link will expire at {{ExpiresAt}}.</p>
                        <p>If you didn't request a password reset, please ignore this email or contact support if you have concerns.</p>
                        <p>Best regards,<br>The {{CompanyName}} Team</p>
                    </div>
                </body>
                </html>
                """
            ),
            ["email-verification"] = new(
                "Verify Your Email Address - {{CompanyName}}",
                """
                <!DOCTYPE html>
                <html>
                <head><meta charset="utf-8"><title>Email Verification</title></head>
                <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;">
                    <div style="background: linear-gradient(135deg, #11998e 0%, #38ef7d 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;">
                        <h1 style="color: white; margin: 0;">Verify Your Email</h1>
                    </div>
                    <div style="background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px;">
                        <p>Hi {{FullName}},</p>
                        <p>Please verify your email address by clicking the button below:</p>
                        <div style="text-align: center; margin: 30px 0;">
                            <a href="{{VerificationUrl}}" style="background: #11998e; color: white; padding: 15px 30px; text-decoration: none; border-radius: 5px; display: inline-block;">Verify Email</a>
                        </div>
                        <p>Best regards,<br>The {{CompanyName}} Team</p>
                    </div>
                </body>
                </html>
                """
            ),
            ["inquiry-notification"] = new(
                "New Service Inquiry: {{ServiceName}}",
                """
                <!DOCTYPE html>
                <html>
                <head><meta charset="utf-8"><title>New Inquiry</title></head>
                <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;">
                    <div style="background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;">
                        <h1 style="color: white; margin: 0;">New Service Inquiry</h1>
                    </div>
                    <div style="background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px;">
                        <p>You have received a new inquiry for <strong>{{ServiceName}}</strong>.</p>
                        <div style="background: white; padding: 20px; border-radius: 5px; margin: 20px 0;">
                            <p><strong>From:</strong> {{InquirerName}}</p>
                            <p><strong>Email:</strong> <a href="mailto:{{InquirerEmail}}">{{InquirerEmail}}</a></p>
                            <p><strong>Company:</strong> {{Company}}</p>
                            <p><strong>Project Description:</strong></p>
                            <p style="background: #f5f5f5; padding: 15px; border-radius: 5px;">{{ProjectDescription}}</p>
                        </div>
                        <p>Please respond to this inquiry at your earliest convenience.</p>
                    </div>
                </body>
                </html>
                """
            ),
            ["inquiry-confirmation"] = new(
                "We Received Your Inquiry - {{CompanyName}}",
                """
                <!DOCTYPE html>
                <html>
                <head><meta charset="utf-8"><title>Inquiry Received</title></head>
                <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;">
                    <div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;">
                        <h1 style="color: white; margin: 0;">Thank You for Your Inquiry!</h1>
                    </div>
                    <div style="background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px;">
                        <p>Hi {{InquirerName}},</p>
                        <p>Thank you for your interest in our <strong>{{ServiceName}}</strong> service. We have received your inquiry and our team will get back to you shortly.</p>
                        <div style="background: white; padding: 20px; border-radius: 5px; margin: 20px 0;">
                            <p><strong>Your Project Description:</strong></p>
                            <p style="background: #f5f5f5; padding: 15px; border-radius: 5px;">{{ProjectDescription}}</p>
                        </div>
                        <p>We typically respond within 24-48 business hours.</p>
                        <p>Best regards,<br>The {{CompanyName}} Team</p>
                    </div>
                </body>
                </html>
                """
            ),
            ["default"] = new(
                "{{Subject}}",
                """
                <!DOCTYPE html>
                <html>
                <head><meta charset="utf-8"><title>{{Subject}}</title></head>
                <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;">
                    <div style="background: #f9f9f9; padding: 30px; border-radius: 10px;">
                        {{Body}}
                    </div>
                </body>
                </html>
                """
            )
        };
    }
}

public record EmailTemplate(string Subject, string Body);
