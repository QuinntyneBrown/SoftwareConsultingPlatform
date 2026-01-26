using MediatR;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.Logout;

public record LogoutCommand(string? RefreshToken) : IRequest<LogoutResponse>;

public record LogoutResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
}
