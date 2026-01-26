using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Identity.Infrastructure.Data;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Users.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponse?>
{
    private readonly IdentityDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCurrentUserQueryHandler(
        IdentityDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetCurrentUserResponse?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)
            ?? _httpContextAccessor.HttpContext?.User.FindFirst("sub");

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return null;
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        if (user == null)
        {
            return null;
        }

        return new GetCurrentUserResponse
        {
            UserId = user.UserId,
            Email = user.Email,
            FullName = user.FullName,
            Phone = user.Phone,
            CompanyName = user.CompanyName,
            AvatarUrl = user.AvatarUrl,
            EmailVerified = user.EmailVerified,
            MfaEnabled = user.MfaEnabled,
            Status = user.Status.ToString(),
            CreatedAt = user.CreatedAt
        };
    }
}
