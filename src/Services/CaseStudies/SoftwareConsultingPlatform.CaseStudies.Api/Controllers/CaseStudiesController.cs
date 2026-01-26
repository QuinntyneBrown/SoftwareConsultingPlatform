using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.CaseStudies.Core.Aggregates;
using SoftwareConsultingPlatform.CaseStudies.Core.ValueObjects;
using SoftwareConsultingPlatform.CaseStudies.Infrastructure.Data;
using Shared.Contracts.CaseStudies;
using Shared.Contracts.Services;
using Shared.Core.Extensions;
using Shared.Core.Interfaces;
using Shared.Messages.Events.CaseStudies;

namespace SoftwareConsultingPlatform.CaseStudies.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CaseStudiesController : ControllerBase
{
    private readonly CaseStudiesDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CaseStudiesController> _logger;

    public CaseStudiesController(CaseStudiesDbContext context, ITenantContext tenantContext, IPublishEndpoint publishEndpoint, ILogger<CaseStudiesController> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CaseStudySummaryDto>>> GetAll()
    {
        var caseStudies = await _context.CaseStudies
            .Where(c => c.TenantId == _tenantContext.TenantId && c.Status == CaseStudyStatus.Published)
            .OrderByDescending(c => c.PublishedDate)
            .Select(c => new CaseStudySummaryDto(c.CaseStudyId, c.ClientName, c.ProjectTitle, c.Slug, c.Overview, null, c.Featured, c.FeaturedOrder))
            .ToListAsync();
        return Ok(caseStudies);
    }

    [HttpGet("featured")]
    public async Task<ActionResult<IEnumerable<CaseStudySummaryDto>>> GetFeatured()
    {
        var caseStudies = await _context.CaseStudies
            .Where(c => c.TenantId == _tenantContext.TenantId && c.Status == CaseStudyStatus.Published && c.Featured)
            .OrderBy(c => c.FeaturedOrder)
            .Take(3)
            .Select(c => new CaseStudySummaryDto(c.CaseStudyId, c.ClientName, c.ProjectTitle, c.Slug, c.Overview, null, c.Featured, c.FeaturedOrder))
            .ToListAsync();
        return Ok(caseStudies);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CaseStudyDetailDto>> GetById(Guid id)
    {
        var cs = await _context.CaseStudies
            .Include(c => c.Technologies).ThenInclude(t => t.Technology)
            .Include(c => c.Images)
            .FirstOrDefaultAsync(c => c.CaseStudyId == id && c.TenantId == _tenantContext.TenantId);
        if (cs == null) return NotFound();
        return Ok(MapToDetail(cs));
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<CaseStudyDetailDto>> GetBySlug(string slug)
    {
        var cs = await _context.CaseStudies
            .Include(c => c.Technologies).ThenInclude(t => t.Technology)
            .Include(c => c.Images)
            .FirstOrDefaultAsync(c => c.Slug == slug && c.TenantId == _tenantContext.TenantId);
        if (cs == null) return NotFound();
        return Ok(MapToDetail(cs));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CaseStudySummaryDto>>> Search([FromQuery] string? q)
    {
        var query = _context.CaseStudies
            .Where(c => c.TenantId == _tenantContext.TenantId && c.Status == CaseStudyStatus.Published);
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(c => c.ProjectTitle.Contains(q) || c.ClientName.Contains(q));
        var results = await query.OrderByDescending(c => c.PublishedDate)
            .Select(c => new CaseStudySummaryDto(c.CaseStudyId, c.ClientName, c.ProjectTitle, c.Slug, c.Overview, null, c.Featured, c.FeaturedOrder))
            .ToListAsync();
        return Ok(results);
    }

    [HttpPost]
    public async Task<ActionResult<CaseStudySummaryDto>> Create([FromBody] CreateCaseStudyRequest request)
    {
        var slug = request.ProjectTitle.ToSlug();
        var cs = new CaseStudy(_tenantContext.TenantId, request.ClientName, request.ProjectTitle, slug);
        cs.Update(request.Overview, request.Challenge, request.Solution, request.Results);
        _context.CaseStudies.Add(cs);
        await _context.SaveChangesAsync();
        await _publishEndpoint.Publish(new CaseStudyCreatedEvent(cs.CaseStudyId, cs.TenantId, cs.ClientName, cs.ProjectTitle, cs.Slug, DateTime.UtcNow));
        return CreatedAtAction(nameof(GetById), new { id = cs.CaseStudyId }, new CaseStudySummaryDto(cs.CaseStudyId, cs.ClientName, cs.ProjectTitle, cs.Slug, cs.Overview, null, cs.Featured, cs.FeaturedOrder));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] CreateCaseStudyRequest request)
    {
        var cs = await _context.CaseStudies.FindAsync(id);
        if (cs == null || cs.TenantId != _tenantContext.TenantId) return NotFound();
        cs.Update(request.Overview, request.Challenge, request.Solution, request.Results);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var cs = await _context.CaseStudies.FindAsync(id);
        if (cs == null || cs.TenantId != _tenantContext.TenantId) return NotFound();
        cs.Archive();
        await _context.SaveChangesAsync();
        await _publishEndpoint.Publish(new CaseStudyArchivedEvent(cs.CaseStudyId, cs.TenantId, DateTime.UtcNow));
        return NoContent();
    }

    [HttpPost("{id:guid}/publish")]
    public async Task<ActionResult> Publish(Guid id)
    {
        var cs = await _context.CaseStudies.FindAsync(id);
        if (cs == null || cs.TenantId != _tenantContext.TenantId) return NotFound();
        cs.Publish();
        await _context.SaveChangesAsync();
        await _publishEndpoint.Publish(new CaseStudyPublishedEvent(cs.CaseStudyId, cs.TenantId, cs.ClientName, cs.ProjectTitle, cs.Slug, DateTime.UtcNow));
        return Ok();
    }

    private static CaseStudyDetailDto MapToDetail(CaseStudy c) =>
        new(c.CaseStudyId, c.ClientName, c.ProjectTitle, c.Slug, c.Overview, c.Challenge, c.Solution, c.Results,
            string.IsNullOrEmpty(c.TestimonialQuote) ? null : new CaseStudyTestimonialDto(c.TestimonialQuote, c.TestimonialAuthor, c.TestimonialAuthorRole, c.TestimonialAuthorCompany),
            new List<CaseStudyMetricDto>(),
            c.Technologies.Select(t => new TechnologyDto(t.Technology!.TechnologyId, t.Technology.Name, t.Technology.Icon, t.Technology.Category)).ToList(),
            c.Images.OrderBy(i => i.DisplayOrder).Select(i => new CaseStudyImageDto(i.ImageUrl, i.ThumbnailUrl, i.Caption, i.DisplayOrder)).ToList());
}
