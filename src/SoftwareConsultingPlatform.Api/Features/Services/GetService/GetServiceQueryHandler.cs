using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;
using SoftwareConsultingPlatform.Core.Models.ServiceAggregate;

namespace SoftwareConsultingPlatform.Api.Features.Services.GetService;

public sealed class GetServiceQueryHandler : IRequestHandler<GetServiceQuery, Service?>
{
    private readonly ISoftwareConsultingPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public GetServiceQueryHandler(
        ISoftwareConsultingPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<Service?> Handle(GetServiceQuery request, CancellationToken cancellationToken)
    {
        return await _context.Services
            .FirstOrDefaultAsync(
                s => s.ServiceId == request.Id && s.TenantId == _tenantContext.TenantId,
                cancellationToken);
    }
}