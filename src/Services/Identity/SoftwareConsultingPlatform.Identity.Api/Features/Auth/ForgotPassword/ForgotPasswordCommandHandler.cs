using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Identity.Infrastructure.Data;
using Shared.Core.Interfaces;
using Shared.Messages.Commands;
using Shared.Messages.Events.Identity;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResponse>
{
    private readonly IdentityDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;

    public ForgotPasswordCommandHandler(
        IdentityDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<ForgotPasswordCommandHandler> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ForgotPasswordResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.TenantId == _tenantContext.TenantId && u.Email == request.Email, cancellationToken);

        // Always return success to prevent email enumeration
        if (user == null)
        {
            _logger.LogWarning("Password reset requested for non-existent email: {Email}", request.Email);
            return new ForgotPasswordResponse
            {
                Message = "If an account with that email exists, a password reset link has been sent."
            };
        }

        // Generate reset token
        var resetToken = Guid.NewGuid().ToString("N");
        var expiry = TimeSpan.FromHours(1);
        user.SetPasswordResetToken(resetToken, expiry);

        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        await _publishEndpoint.Publish(new PasswordResetRequestedEvent(
            user.UserId,
            user.TenantId,
            user.Email,
            resetToken,
            DateTime.UtcNow.Add(expiry)
        ), cancellationToken);

        // Send email
        var resetUrl = $"https://app.example.com/reset-password?token={resetToken}";
        await _publishEndpoint.Publish(new SendPasswordResetEmailCommand(
            user.TenantId,
            user.Email,
            resetUrl,
            DateTime.UtcNow.Add(expiry)
        ), cancellationToken);

        _logger.LogInformation("Password reset token generated for user: {Email}", request.Email);

        return new ForgotPasswordResponse
        {
            Message = "If an account with that email exists, a password reset link has been sent."
        };
    }
}
