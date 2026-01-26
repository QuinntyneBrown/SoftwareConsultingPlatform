using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Identity.Infrastructure.Data;
using Shared.Messages.Events.Identity;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.VerifyEmail;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, VerifyEmailResponse>
{
    private readonly IdentityDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<VerifyEmailCommandHandler> _logger;

    public VerifyEmailCommandHandler(
        IdentityDbContext context,
        IPublishEndpoint publishEndpoint,
        ILogger<VerifyEmailCommandHandler> logger)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<VerifyEmailResponse> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.EmailVerificationToken == request.Token, cancellationToken);

        if (user == null)
        {
            return new VerifyEmailResponse
            {
                Success = false,
                Message = "Invalid verification token."
            };
        }

        if (user.EmailVerified)
        {
            return new VerifyEmailResponse
            {
                Success = true,
                Message = "Email is already verified."
            };
        }

        user.VerifyEmail();
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        await _publishEndpoint.Publish(new UserEmailVerifiedEvent(
            user.UserId,
            user.TenantId,
            DateTime.UtcNow
        ), cancellationToken);

        _logger.LogInformation("Email verified for user: {UserId}", user.UserId);

        return new VerifyEmailResponse
        {
            Success = true,
            Message = "Email verified successfully."
        };
    }
}
