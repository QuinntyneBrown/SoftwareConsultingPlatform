using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;
using SoftwareConsultingPlatform.Core.Models.ServiceAggregate;
using SoftwareConsultingPlatform.Core.Models.ServiceAggregate.Enums;

namespace SoftwareConsultingPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ISoftwareConsultingPlatformContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(
        ISoftwareConsultingPlatformContext context,
        ITenantContext tenantContext,
        ILogger<ServicesController> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetServices(CancellationToken cancellationToken)
    {
        var services = await _context.Services
            .Where(s => s.TenantId == _tenantContext.TenantId && s.Status == ServiceStatus.Published)
            .OrderBy(s => s.DisplayOrder)
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

        return Ok(services);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetService(Guid id, CancellationToken cancellationToken)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.ServiceId == id && s.TenantId == _tenantContext.TenantId, cancellationToken);

        if (service == null)
            return NotFound();

        return Ok(service);
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<IActionResult> GetServiceBySlug(string slug, CancellationToken cancellationToken)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Slug == slug && s.TenantId == _tenantContext.TenantId && s.Status == ServiceStatus.Published, cancellationToken);

        if (service == null)
            return NotFound();

        return Ok(service);
    }

    [HttpPost("{id:guid}/inquiries")]
    public async Task<IActionResult> SubmitInquiry(
        Guid id,
        [FromBody] SubmitInquiryRequest request,
        CancellationToken cancellationToken)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.ServiceId == id && s.TenantId == _tenantContext.TenantId, cancellationToken);

        if (service == null)
            return NotFound();

        var inquiry = new ServiceInquiry(
            _tenantContext.TenantId,
            id,
            request.Name,
            request.Email,
            request.Company,
            request.ProjectDescription);

        _context.ServiceInquiries.Add(inquiry);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Service inquiry submitted for service {ServiceId}", id);

        return StatusCode(201, new { message = "Inquiry submitted successfully", inquiryId = inquiry.ServiceInquiryId });
    }
}

public class SubmitInquiryRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string ProjectDescription { get; set; } = string.Empty;
}
