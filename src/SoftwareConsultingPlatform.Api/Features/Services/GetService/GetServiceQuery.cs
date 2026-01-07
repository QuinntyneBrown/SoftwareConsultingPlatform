using MediatR;
using SoftwareConsultingPlatform.Core.Models.ServiceAggregate;

namespace SoftwareConsultingPlatform.Api.Features.Services.GetService;

public sealed class GetServiceQuery : IRequest<Service?>
{
    public Guid Id { get; set; }
}