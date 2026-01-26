using MediatR;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.ResetPassword;

public record ResetPasswordCommand(string Token, string NewPassword) : IRequest<ResetPasswordResponse>;

public record ResetPasswordResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
}
