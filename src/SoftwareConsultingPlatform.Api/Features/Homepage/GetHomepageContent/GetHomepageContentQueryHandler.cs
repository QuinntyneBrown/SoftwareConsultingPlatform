using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;

namespace SoftwareConsultingPlatform.Api.Features.Homepage.GetHomepageContent;

public sealed class GetHomepageContentQueryHandler : IRequestHandler<GetHomepageContentQuery, object?>
{
    private readonly ISoftwareConsultingPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public GetHomepageContentQueryHandler(
        ISoftwareConsultingPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<object?> Handle(GetHomepageContentQuery request, CancellationToken cancellationToken)
    {
        return await _context.HomepageContents
            .Where(h => h.TenantId == _tenantContext.TenantId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}