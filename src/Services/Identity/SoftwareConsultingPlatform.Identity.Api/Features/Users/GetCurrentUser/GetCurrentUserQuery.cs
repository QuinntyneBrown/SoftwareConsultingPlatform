using MediatR;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Users.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<GetCurrentUserResponse?>;

public record GetCurrentUserResponse
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string? CompanyName { get; init; }
    public string? AvatarUrl { get; init; }
    public bool EmailVerified { get; init; }
    public bool MfaEnabled { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
