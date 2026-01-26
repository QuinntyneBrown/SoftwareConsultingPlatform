using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SoftwareConsultingPlatform.Content.Core.Aggregates;
using SoftwareConsultingPlatform.Content.Infrastructure.Data;
using Shared.Contracts.CaseStudies;
using Shared.Contracts.Content;
using Shared.Contracts.Services;
using Shared.Core.Interfaces;
using Shared.Messages.Events.Content;
using System.Text.Json;

namespace SoftwareConsultingPlatform.Content.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HomepageController : ControllerBase
{
    private readonly ContentDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IDistributedCache _cache;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HomepageController> _logger;

    public HomepageController(ContentDbContext context, ITenantContext tenantContext, IPublishEndpoint publishEndpoint, IDistributedCache cache, IHttpClientFactory httpClientFactory, ILogger<HomepageController> logger)
    {
        _context = context; _tenantContext = tenantContext; _publishEndpoint = publishEndpoint; _cache = cache; _httpClientFactory = httpClientFactory; _logger = logger;
    }

    [HttpGet("content")]
    public async Task<ActionResult<HomepageContentDto>> GetContent()
    {
        var content = await _context.HomepageContents.FirstOrDefaultAsync(c => c.TenantId == _tenantContext.TenantId);
        if (content == null)
        {
            content = new HomepageContent(_tenantContext.TenantId);
            _context.HomepageContents.Add(content);
            await _context.SaveChangesAsync();
        }
        return Ok(MapToDto(content));
    }

    [HttpPut("content")]
    [Authorize]
    public async Task<ActionResult> UpdateContent([FromBody] UpdateHomepageContentRequest request)
    {
        var content = await _context.HomepageContents.FirstOrDefaultAsync(c => c.TenantId == _tenantContext.TenantId);
        if (content == null)
        {
            content = new HomepageContent(_tenantContext.TenantId);
            _context.HomepageContents.Add(content);
        }
        var userId = User.FindFirst("sub")?.Value;
        if (request.Hero != null) content.UpdateHero(request.Hero.Title, request.Hero.Subtitle, request.Hero.ImageUrl, request.Hero.CtaText, request.Hero.CtaUrl, userId);
        if (request.Services != null) content.UpdateServices(JsonSerializer.Serialize(request.Services), userId);
        if (request.Testimonials != null) content.UpdateTestimonials(JsonSerializer.Serialize(request.Testimonials), userId);
        if (request.Sections != null) content.UpdateSections(JsonSerializer.Serialize(request.Sections), userId);
        await _context.SaveChangesAsync();
        await _publishEndpoint.Publish(new HomepageContentUpdatedEvent(content.HomepageContentId, content.TenantId, userId ?? "system", DateTime.UtcNow));
        await _cache.RemoveAsync($"homepage:{_tenantContext.TenantId}");
        return Ok();
    }

    [HttpGet("featured-services")]
    public async Task<ActionResult<IEnumerable<ServiceSummaryDto>>> GetFeaturedServices()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("services-service");
            client.DefaultRequestHeaders.Add("X-Tenant-Id", _tenantContext.TenantId.ToString());
            var response = await client.GetAsync("/services/featured");
            if (response.IsSuccessStatusCode)
            {
                var services = await response.Content.ReadFromJsonAsync<IEnumerable<ServiceSummaryDto>>();
                return Ok(services ?? Enumerable.Empty<ServiceSummaryDto>());
            }
        }
        catch (Exception ex) { _logger.LogWarning(ex, "Failed to fetch featured services"); }
        return Ok(Enumerable.Empty<ServiceSummaryDto>());
    }

    [HttpGet("featured-case-studies")]
    public async Task<ActionResult<IEnumerable<CaseStudySummaryDto>>> GetFeaturedCaseStudies()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("casestudies-service");
            client.DefaultRequestHeaders.Add("X-Tenant-Id", _tenantContext.TenantId.ToString());
            var response = await client.GetAsync("/casestudies/featured");
            if (response.IsSuccessStatusCode)
            {
                var caseStudies = await response.Content.ReadFromJsonAsync<IEnumerable<CaseStudySummaryDto>>();
                return Ok(caseStudies ?? Enumerable.Empty<CaseStudySummaryDto>());
            }
        }
        catch (Exception ex) { _logger.LogWarning(ex, "Failed to fetch featured case studies"); }
        return Ok(Enumerable.Empty<CaseStudySummaryDto>());
    }

    private static HomepageContentDto MapToDto(HomepageContent c) => new(
        c.HomepageContentId,
        new HeroSectionDto(c.HeroTitle, c.HeroSubtitle, c.HeroImageUrl, c.HeroCtaText, c.HeroCtaUrl),
        Deserialize<List<HomepageServiceDto>>(c.ServicesJson) ?? new(),
        Deserialize<List<HomepageTestimonialDto>>(c.TestimonialsJson) ?? new(),
        Deserialize<List<HomepageSectionDto>>(c.SectionsJson) ?? new());

    private static T? Deserialize<T>(string? json) where T : class
    {
        if (string.IsNullOrEmpty(json)) return null;
        try { return JsonSerializer.Deserialize<T>(json); } catch { return null; }
    }
}
