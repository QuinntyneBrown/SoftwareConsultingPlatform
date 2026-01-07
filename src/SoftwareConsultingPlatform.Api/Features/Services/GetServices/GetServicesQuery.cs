using MediatR;

namespace SoftwareConsultingPlatform.Api.Features.Services.GetServices;

public sealed class GetServicesQuery : IRequest<IReadOnlyList<GetServicesResponseItem>>
{
}