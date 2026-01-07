using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;
using SoftwareConsultingPlatform.Core.Models.ServiceAggregate.Enums;

namespace SoftwareConsultingPlatform.Api.Features.Services.GetServices;

public sealed class GetServicesQueryHandler : IRequestHandler<GetServicesQuery, IReadOnlyList<GetServicesResponseItem>>
{
    private readonly ISoftwareConsultingPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public GetServicesQueryHandler(
        ISoftwareConsultingPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyList<GetServicesResponseItem>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Services
            .Where(s => s.TenantId == _tenantContext.TenantId && s.Status == ServiceStatus.Published)
            .OrderBy(s => s.DisplayOrder)
            .Select(s => new GetServicesResponseItem
            {
                ServiceId = s.ServiceId,
                Name = s.Name,
                Slug = s.Slug,
                Tagline = s.Tagline,
                Overview = s.Overview,
                IconUrl = s.IconUrl
            })
            .ToListAsync(cancellationToken);
    }
}