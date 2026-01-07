using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;

namespace SoftwareConsultingPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomepageController : ControllerBase
{
    private readonly ISoftwareConsultingPlatformContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<HomepageController> _logger;

    public HomepageController(
        ISoftwareConsultingPlatformContext context,
        ITenantContext tenantContext,
        ILogger<HomepageController> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    [HttpGet("content")]
    public async Task<IActionResult> GetContent(CancellationToken cancellationToken)
    {
        var content = await _context.HomepageContents
            .Where(h => h.TenantId == _tenantContext.TenantId)
            .FirstOrDefaultAsync(cancellationToken);

        if (content == null)
            return NotFound();

        return Ok(content);
    }

    [HttpGet("featured-case-studies")]
    public async Task<IActionResult> GetFeaturedCaseStudies(CancellationToken cancellationToken)
    {
        var featuredCaseStudies = await _context.CaseStudies
            .Where(cs => cs.TenantId == _tenantContext.TenantId && cs.Status == Core.Models.CaseStudyAggregate.Enums.CaseStudyStatus.Published && cs.Featured)
            .OrderBy(cs => cs.FeaturedOrder)
            .Take(3)
            .Select(cs => new
            {
                cs.CaseStudyId,
                cs.ClientName,
                cs.ProjectTitle,
                cs.Slug,
                cs.Overview
            })
            .ToListAsync(cancellationToken);

        return Ok(featuredCaseStudies);
    }

    [HttpGet("featured-services")]
    public async Task<IActionResult> GetFeaturedServices(CancellationToken cancellationToken)
    {
        var featuredServices = await _context.Services
            .Where(s => s.TenantId == _tenantContext.TenantId && s.Status == Core.Models.ServiceAggregate.Enums.ServiceStatus.Published && s.Featured)
            .OrderBy(s => s.DisplayOrder)
            .Take(3)
            .Select(s => new
            {
                s.ServiceId,
                s.Name,
                s.Slug,
                s.Tagline,
                s.Overview,
                s.IconUrl
            })
            .ToListAsync(cancellationToken);

        return Ok(featuredServices);
    }
}
