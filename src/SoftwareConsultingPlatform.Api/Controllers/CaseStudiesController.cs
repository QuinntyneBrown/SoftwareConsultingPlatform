using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;
using SoftwareConsultingPlatform.Core.Models.CaseStudyAggregate.Enums;

namespace SoftwareConsultingPlatform.Api.Controllers;

[ApiController]
[Route("api/case-studies")]
public class CaseStudiesController : ControllerBase
{
    private readonly ISoftwareConsultingPlatformContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CaseStudiesController> _logger;

    public CaseStudiesController(
        ISoftwareConsultingPlatformContext context,
        ITenantContext tenantContext,
        ILogger<CaseStudiesController> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetCaseStudies(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        CancellationToken cancellationToken = default)
    {
        pageSize = Math.Min(pageSize, 50);

        var query = _context.CaseStudies
            .Where(cs => cs.TenantId == _tenantContext.TenantId && cs.Status == CaseStudyStatus.Published);

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var caseStudies = await query
            .OrderByDescending(cs => cs.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(cs => new
            {
                cs.CaseStudyId,
                cs.ClientName,
                cs.ProjectTitle,
                cs.Slug,
                cs.Overview,
                cs.PublishedDate
            })
            .ToListAsync(cancellationToken);

        return Ok(new
        {
            data = caseStudies,
            pagination = new
            {
                currentPage = page,
                pageSize,
                totalCount,
                totalPages,
                hasNext = page < totalPages,
                hasPrevious = page > 1
            }
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCaseStudy(Guid id, CancellationToken cancellationToken)
    {
        var caseStudy = await _context.CaseStudies
            .FirstOrDefaultAsync(cs => cs.CaseStudyId == id && cs.TenantId == _tenantContext.TenantId, cancellationToken);

        if (caseStudy == null)
            return NotFound();

        return Ok(caseStudy);
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<IActionResult> GetCaseStudyBySlug(string slug, CancellationToken cancellationToken)
    {
        var caseStudy = await _context.CaseStudies
            .FirstOrDefaultAsync(cs => cs.Slug == slug && cs.TenantId == _tenantContext.TenantId && cs.Status == CaseStudyStatus.Published, cancellationToken);

        if (caseStudy == null)
            return NotFound();

        return Ok(caseStudy);
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeaturedCaseStudies(CancellationToken cancellationToken)
    {
        var caseStudies = await _context.CaseStudies
            .Where(cs => cs.TenantId == _tenantContext.TenantId && cs.Status == CaseStudyStatus.Published && cs.Featured)
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

        return Ok(caseStudies);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCaseStudies(
        [FromQuery] string q,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { message = "Search query is required" });

        pageSize = Math.Min(pageSize, 50);

        var query = _context.CaseStudies
            .Where(cs => cs.TenantId == _tenantContext.TenantId 
                && cs.Status == CaseStudyStatus.Published
                && (cs.ProjectTitle.Contains(q) || cs.ClientName.Contains(q) || cs.Overview.Contains(q)));

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var caseStudies = await query
            .OrderByDescending(cs => cs.PublishedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(cs => new
            {
                cs.CaseStudyId,
                cs.ClientName,
                cs.ProjectTitle,
                cs.Slug,
                cs.Overview
            })
            .ToListAsync(cancellationToken);

        return Ok(new
        {
            data = caseStudies,
            pagination = new
            {
                currentPage = page,
                pageSize,
                totalCount,
                totalPages
            }
        });
    }
}
