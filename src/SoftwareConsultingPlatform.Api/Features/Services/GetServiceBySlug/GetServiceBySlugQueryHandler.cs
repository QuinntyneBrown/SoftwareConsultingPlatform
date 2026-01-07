using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;
using SoftwareConsultingPlatform.Core.Models.ServiceAggregate;
using SoftwareConsultingPlatform.Core.Models.ServiceAggregate.Enums;

namespace SoftwareConsultingPlatform.Api.Features.Services.GetServiceBySlug;

public sealed class GetServiceBySlugQueryHandler : IRequestHandler<GetServiceBySlugQuery, Service?>
{
    private readonly ISoftwareConsultingPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public GetServiceBySlugQueryHandler(
        ISoftwareConsultingPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<Service?> Handle(GetServiceBySlugQuery request, CancellationToken cancellationToken)
    {
        return await _context.Services
            .FirstOrDefaultAsync(
                s => s.Slug == request.Slug
                    && s.TenantId == _tenantContext.TenantId
                    && s.Status == ServiceStatus.Published,
                cancellationToken);
    }
}