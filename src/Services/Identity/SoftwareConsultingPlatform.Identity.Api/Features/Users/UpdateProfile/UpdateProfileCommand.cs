using MediatR;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Users.UpdateProfile;

public record UpdateProfileCommand(
    string? FullName,
    string? Phone,
    string? CompanyName,
    string? AvatarUrl
) : IRequest<UpdateProfileResponse>;

public record UpdateProfileResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
}
