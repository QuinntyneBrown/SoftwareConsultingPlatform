using MediatR;
using SoftwareConsultingPlatform.Core.Models.ServiceAggregate;

namespace SoftwareConsultingPlatform.Api.Features.Services.GetServiceBySlug;

public sealed class GetServiceBySlugQuery : IRequest<Service?>
{
    public string Slug { get; set; } = string.Empty;
}