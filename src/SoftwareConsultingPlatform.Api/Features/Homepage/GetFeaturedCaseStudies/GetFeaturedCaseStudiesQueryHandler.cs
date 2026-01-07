using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;
using SoftwareConsultingPlatform.Core.Models.CaseStudyAggregate.Enums;

namespace SoftwareConsultingPlatform.Api.Features.Homepage.GetFeaturedCaseStudies;

public sealed class GetFeaturedCaseStudiesQueryHandler : IRequestHandler<GetFeaturedCaseStudiesQuery, IReadOnlyList<GetFeaturedCaseStudiesResponseItem>>
{
    private readonly ISoftwareConsultingPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public GetFeaturedCaseStudiesQueryHandler(
        ISoftwareConsultingPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyList<GetFeaturedCaseStudiesResponseItem>> Handle(
        GetFeaturedCaseStudiesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.CaseStudies
            .Where(cs =>
                cs.TenantId == _tenantContext.TenantId
                && cs.Status == CaseStudyStatus.Published
                && cs.Featured)
            .OrderBy(cs => cs.FeaturedOrder)
            .Take(3)
            .Select(cs => new GetFeaturedCaseStudiesResponseItem
            {
                CaseStudyId = cs.CaseStudyId,
                ClientName = cs.ClientName,
                ProjectTitle = cs.ProjectTitle,
                Slug = cs.Slug,
                Overview = cs.Overview
            })
            .ToListAsync(cancellationToken);
    }
}