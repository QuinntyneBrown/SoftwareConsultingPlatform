using MediatR;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginResponse?>;
