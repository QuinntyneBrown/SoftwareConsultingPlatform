using MediatR;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.VerifyEmail;

public record VerifyEmailCommand(string Token) : IRequest<VerifyEmailResponse>;

public record VerifyEmailResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
}
