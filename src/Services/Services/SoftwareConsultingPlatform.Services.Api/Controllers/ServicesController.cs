using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Services.Core.Aggregates;
using SoftwareConsultingPlatform.Services.Core.ValueObjects;
using SoftwareConsultingPlatform.Services.Infrastructure.Data;
using Shared.Contracts.Services;
using Shared.Core.Extensions;
using Shared.Core.Interfaces;
using Shared.Messages.Commands;
using Shared.Messages.Events.Services;

namespace SoftwareConsultingPlatform.Services.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ServicesDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(
        ServicesDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<ServicesController> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceSummaryDto>>> GetAll()
    {
        var services = await _context.Services
            .Where(s => s.TenantId == _tenantContext.TenantId && s.Status == ServiceStatus.Published)
            .OrderBy(s => s.DisplayOrder)
            .Select(s => new ServiceSummaryDto(
                s.ServiceId,
                s.Name,
                s.Slug,
                s.Tagline,
                s.IconUrl,
                s.Featured,
                s.DisplayOrder))
            .ToListAsync();

        return Ok(services);
    }

    [HttpGet("featured")]
    public async Task<ActionResult<IEnumerable<ServiceSummaryDto>>> GetFeatured()
    {
        var services = await _context.Services
            .Where(s => s.TenantId == _tenantContext.TenantId && s.Status == ServiceStatus.Published && s.Featured)
            .OrderBy(s => s.DisplayOrder)
            .Take(6)
            .Select(s => new ServiceSummaryDto(
                s.ServiceId,
                s.Name,
                s.Slug,
                s.Tagline,
                s.IconUrl,
                s.Featured,
                s.DisplayOrder))
            .ToListAsync();

        return Ok(services);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceDetailDto>> GetById(Guid id)
    {
        var service = await _context.Services
            .Include(s => s.Technologies)
                .ThenInclude(st => st.Technology)
            .Include(s => s.Faqs)
            .FirstOrDefaultAsync(s => s.ServiceId == id && s.TenantId == _tenantContext.TenantId);

        if (service == null)
        {
            return NotFound();
        }

        return Ok(MapToDetailDto(service));
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<ServiceDetailDto>> GetBySlug(string slug)
    {
        var service = await _context.Services
            .Include(s => s.Technologies)
                .ThenInclude(st => st.Technology)
            .Include(s => s.Faqs)
            .FirstOrDefaultAsync(s => s.Slug == slug && s.TenantId == _tenantContext.TenantId);

        if (service == null)
        {
            return NotFound();
        }

        return Ok(MapToDetailDto(service));
    }

    [HttpPost("{id:guid}/inquiries")]
    public async Task<ActionResult> SubmitInquiry(Guid id, [FromBody] ServiceInquiryRequest request)
    {
        var service = await _context.Services.FindAsync(id);

        if (service == null || service.TenantId != _tenantContext.TenantId)
        {
            return NotFound();
        }

        var inquiry = new ServiceInquiry(
            service.ServiceId,
            service.TenantId,
            request.Name,
            request.Email,
            request.Company,
            request.ProjectDescription);

        _context.ServiceInquiries.Add(inquiry);
        await _context.SaveChangesAsync();

        // Publish event
        await _publishEndpoint.Publish(new ServiceInquirySubmittedEvent(
            inquiry.ServiceInquiryId,
            inquiry.ServiceId,
            inquiry.TenantId,
            inquiry.Name,
            inquiry.Email,
            inquiry.Company,
            inquiry.ProjectDescription,
            DateTime.UtcNow));

        // Send notification
        await _publishEndpoint.Publish(new SendInquiryNotificationCommand(
            inquiry.TenantId,
            inquiry.ServiceId,
            service.Name,
            inquiry.Name,
            inquiry.Email,
            inquiry.ProjectDescription));

        _logger.LogInformation("Service inquiry submitted: {InquiryId}", inquiry.ServiceInquiryId);

        return Ok(new { message = "Inquiry submitted successfully" });
    }

    [HttpPost]
    public async Task<ActionResult<ServiceSummaryDto>> Create([FromBody] CreateServiceRequest request)
    {
        var slug = request.Name.ToSlug();

        var existing = await _context.Services
            .FirstOrDefaultAsync(s => s.TenantId == _tenantContext.TenantId && s.Slug == slug);

        if (existing != null)
        {
            return BadRequest("A service with this name already exists");
        }

        var service = new Service(_tenantContext.TenantId, request.Name, slug);
        service.Update(request.Tagline, request.Overview, request.IconUrl);

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new ServiceCreatedEvent(
            service.ServiceId,
            service.TenantId,
            service.Name,
            service.Slug,
            DateTime.UtcNow));

        return CreatedAtAction(nameof(GetById), new { id = service.ServiceId },
            new ServiceSummaryDto(
                service.ServiceId,
                service.Name,
                service.Slug,
                service.Tagline,
                service.IconUrl,
                service.Featured,
                service.DisplayOrder));
    }

    [HttpPost("{id:guid}/publish")]
    public async Task<ActionResult> Publish(Guid id)
    {
        var service = await _context.Services.FindAsync(id);

        if (service == null || service.TenantId != _tenantContext.TenantId)
        {
            return NotFound();
        }

        service.Publish();
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new ServicePublishedEvent(
            service.ServiceId,
            service.TenantId,
            service.Name,
            service.Slug,
            DateTime.UtcNow));

        return Ok();
    }

    [HttpPost("{id:guid}/archive")]
    public async Task<ActionResult> Archive(Guid id)
    {
        var service = await _context.Services.FindAsync(id);

        if (service == null || service.TenantId != _tenantContext.TenantId)
        {
            return NotFound();
        }

        service.Archive();
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new ServiceArchivedEvent(
            service.ServiceId,
            service.TenantId,
            DateTime.UtcNow));

        return Ok();
    }

    private static ServiceDetailDto MapToDetailDto(Service service)
    {
        return new ServiceDetailDto(
            service.ServiceId,
            service.Name,
            service.Slug,
            service.Tagline,
            service.Overview,
            service.IconUrl,
            DeserializeList(service.WhatWeDoJson),
            DeserializeList(service.HowWeWorkJson),
            DeserializeList(service.BenefitsJson),
            new List<PricingModelDto>(),
            new List<EngagementTypeDto>(),
            service.Technologies.Select(t => new TechnologyDto(
                t.Technology!.TechnologyId,
                t.Technology.Name,
                t.Technology.Icon,
                t.Technology.Category)).ToList(),
            service.Faqs.OrderBy(f => f.DisplayOrder).Select(f => new FaqDto(
                f.Question,
                f.Answer,
                f.DisplayOrder)).ToList());
    }

    private static List<string> DeserializeList(string? json)
    {
        if (string.IsNullOrEmpty(json)) return new List<string>();
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }
}
