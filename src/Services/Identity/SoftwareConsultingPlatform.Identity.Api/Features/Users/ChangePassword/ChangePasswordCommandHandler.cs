using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Identity.Infrastructure.Data;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Users.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResponse>
{
    private readonly IdentityDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ChangePasswordCommandHandler> _logger;

    public ChangePasswordCommandHandler(
        IdentityDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ILogger<ChangePasswordCommandHandler> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<ChangePasswordResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)
            ?? _httpContextAccessor.HttpContext?.User.FindFirst("sub");

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new ChangePasswordResponse { Success = false, Message = "User not found" };
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        if (user == null)
        {
            return new ChangePasswordResponse { Success = false, Message = "User not found" };
        }

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
        {
            return new ChangePasswordResponse { Success = false, Message = "Current password is incorrect" };
        }

        var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatePassword(newPasswordHash);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Password changed for user: {UserId}", userId);

        return new ChangePasswordResponse { Success = true, Message = "Password changed successfully" };
    }
}
