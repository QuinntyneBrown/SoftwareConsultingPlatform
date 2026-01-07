using MediatR;

namespace SoftwareConsultingPlatform.Api.Features.Homepage.GetFeaturedCaseStudies;

public sealed class GetFeaturedCaseStudiesQuery : IRequest<IReadOnlyList<GetFeaturedCaseStudiesResponseItem>>
{
}