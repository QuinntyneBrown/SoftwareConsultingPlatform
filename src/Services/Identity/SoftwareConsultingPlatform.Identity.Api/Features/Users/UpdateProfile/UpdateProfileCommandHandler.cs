using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Identity.Infrastructure.Data;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Users.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UpdateProfileResponse>
{
    private readonly IdentityDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UpdateProfileCommandHandler> _logger;

    public UpdateProfileCommandHandler(
        IdentityDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UpdateProfileCommandHandler> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<UpdateProfileResponse> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)
            ?? _httpContextAccessor.HttpContext?.User.FindFirst("sub");

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new UpdateProfileResponse { Success = false, Message = "User not found" };
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        if (user == null)
        {
            return new UpdateProfileResponse { Success = false, Message = "User not found" };
        }

        user.UpdateProfile(request.FullName, request.Phone, request.CompanyName);

        if (request.AvatarUrl != null)
        {
            user.UpdateAvatar(request.AvatarUrl);
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Profile updated for user: {UserId}", userId);

        return new UpdateProfileResponse { Success = true, Message = "Profile updated successfully" };
    }
}
