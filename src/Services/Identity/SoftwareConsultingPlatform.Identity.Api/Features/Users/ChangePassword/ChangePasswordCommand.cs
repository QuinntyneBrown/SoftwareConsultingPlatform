using MediatR;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Users.ChangePassword;

public record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword
) : IRequest<ChangePasswordResponse>;

public record ChangePasswordResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
}
