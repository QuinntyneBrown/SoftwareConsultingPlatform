using MediatR;

namespace SoftwareConsultingPlatform.Api.Features.Homepage.GetFeaturedServices;

public sealed class GetFeaturedServicesQuery : IRequest<IReadOnlyList<GetFeaturedServicesResponseItem>>
{
}