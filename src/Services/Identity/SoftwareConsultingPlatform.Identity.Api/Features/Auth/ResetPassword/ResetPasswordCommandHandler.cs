using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Identity.Infrastructure.Data;
using Shared.Core.Interfaces;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
{
    private readonly IdentityDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(
        IdentityDbContext context,
        ITenantContext tenantContext,
        ILogger<ResetPasswordCommandHandler> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.TenantId == _tenantContext.TenantId &&
                                       u.ValidatePasswordResetToken(request.Token), cancellationToken);

        if (user == null)
        {
            return new ResetPasswordResponse
            {
                Success = false,
                Message = "Invalid or expired reset token."
            };
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatePassword(passwordHash);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Password reset successful for user: {UserId}", user.UserId);

        return new ResetPasswordResponse
        {
            Success = true,
            Message = "Password has been reset successfully."
        };
    }
}
