using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Identity.Infrastructure.Data;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutResponse>
{
    private readonly IdentityDbContext _context;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(
        IdentityDbContext context,
        ILogger<LogoutCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<LogoutResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.RefreshToken))
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

            if (refreshToken != null)
            {
                refreshToken.Revoke();
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        return new LogoutResponse
        {
            Success = true,
            Message = "Logged out successfully."
        };
    }
}
