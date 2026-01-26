using MediatR;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FullName,
    string? Phone,
    string? CompanyName
) : IRequest<RegisterResponse>;
