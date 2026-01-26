using MediatR;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<ForgotPasswordResponse>;

public record ForgotPasswordResponse
{
    public string Message { get; init; } = string.Empty;
}
