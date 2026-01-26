using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Contracts.Tenant;
using Shared.Messages.Events.Tenant;
using SoftwareConsultingPlatform.Tenant.Core.ValueObjects;
using SoftwareConsultingPlatform.Tenant.Infrastructure.Data;
using System.Text.Json;

namespace SoftwareConsultingPlatform.Tenant.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TenantsController : ControllerBase
{
    private readonly TenantDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IDistributedCache _cache;
    private readonly ILogger<TenantsController> _logger;

    public TenantsController(
        TenantDbContext context,
        IPublishEndpoint publishEndpoint,
        IDistributedCache cache,
        ILogger<TenantsController> logger)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
        _cache = cache;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TenantDto>>> GetAll()
    {
        var tenants = await _context.Tenants
            .Select(t => MapToDto(t))
            .ToListAsync();

        return Ok(tenants);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TenantDto>> GetById(Guid id)
    {
        var cacheKey = $"tenant:{id}";
        var cached = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cached))
        {
            return Ok(JsonSerializer.Deserialize<TenantDto>(cached));
        }

        var tenant = await _context.Tenants.FindAsync(id);

        if (tenant == null)
        {
            return NotFound();
        }

        var dto = MapToDto(tenant);
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });

        return Ok(dto);
    }

    [HttpGet("by-subdomain/{subdomain}")]
    public async Task<ActionResult<TenantDto>> GetBySubdomain(string subdomain)
    {
        var cacheKey = $"tenant:subdomain:{subdomain}";
        var cached = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cached))
        {
            return Ok(JsonSerializer.Deserialize<TenantDto>(cached));
        }

        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Subdomain == subdomain);

        if (tenant == null)
        {
            return NotFound();
        }

        var dto = MapToDto(tenant);
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });

        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<TenantDto>> Create([FromBody] CreateTenantRequest request)
    {
        var existing = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Subdomain == request.Subdomain);

        if (existing != null)
        {
            return BadRequest("A tenant with this subdomain already exists");
        }

        var tenant = new Core.Aggregates.Tenant(request.Name, request.Subdomain);
        tenant.SetCustomDomain(request.CustomDomain);
        tenant.UpdateContactInfo(request.ContactEmail, request.ContactPhone, null, null);

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new TenantCreatedEvent(
            tenant.TenantId,
            tenant.Name,
            tenant.Subdomain,
            DateTime.UtcNow));

        _logger.LogInformation("Tenant created: {TenantId}", tenant.TenantId);

        return CreatedAtAction(nameof(GetById), new { id = tenant.TenantId }, MapToDto(tenant));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<TenantDto>> Update(Guid id, [FromBody] UpdateTenantRequest request)
    {
        var tenant = await _context.Tenants.FindAsync(id);

        if (tenant == null)
        {
            return NotFound();
        }

        if (request.Branding != null)
        {
            tenant.UpdateBranding(
                request.Branding.LogoUrl,
                request.Branding.FaviconUrl,
                request.Branding.PrimaryColor,
                request.Branding.SecondaryColor,
                request.Branding.FontFamily);

            await _publishEndpoint.Publish(new TenantBrandingUpdatedEvent(
                tenant.TenantId,
                request.Branding.LogoUrl,
                request.Branding.PrimaryColor,
                request.Branding.SecondaryColor));
        }

        if (request.Contact != null)
        {
            tenant.UpdateContactInfo(
                request.Contact.Email,
                request.Contact.Phone,
                request.Contact.Address,
                request.Contact.SupportEmail);
        }

        tenant.SetCustomDomain(request.CustomDomain);

        await _context.SaveChangesAsync();

        // Invalidate cache
        await _cache.RemoveAsync($"tenant:{id}");
        await _cache.RemoveAsync($"tenant:subdomain:{tenant.Subdomain}");

        return Ok(MapToDto(tenant));
    }

    [HttpPost("{id:guid}/activate")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult> Activate(Guid id)
    {
        var tenant = await _context.Tenants.FindAsync(id);

        if (tenant == null)
        {
            return NotFound();
        }

        tenant.Activate();
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new TenantActivatedEvent(tenant.TenantId, DateTime.UtcNow));

        await _cache.RemoveAsync($"tenant:{id}");
        await _cache.RemoveAsync($"tenant:subdomain:{tenant.Subdomain}");

        return Ok();
    }

    [HttpPost("{id:guid}/suspend")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult> Suspend(Guid id, [FromBody] SuspendTenantRequest request)
    {
        var tenant = await _context.Tenants.FindAsync(id);

        if (tenant == null)
        {
            return NotFound();
        }

        tenant.Suspend();
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new TenantSuspendedEvent(
            tenant.TenantId,
            request.Reason,
            DateTime.UtcNow));

        await _cache.RemoveAsync($"tenant:{id}");
        await _cache.RemoveAsync($"tenant:subdomain:{tenant.Subdomain}");

        return Ok();
    }

    private static TenantDto MapToDto(Core.Aggregates.Tenant tenant)
    {
        return new TenantDto(
            tenant.TenantId,
            tenant.Name,
            tenant.Subdomain,
            tenant.CustomDomain,
            tenant.Status.ToString(),
            new TenantBrandingDto(
                tenant.LogoUrl,
                tenant.FaviconUrl,
                tenant.PrimaryColor,
                tenant.SecondaryColor,
                tenant.FontFamily),
            new TenantContactDto(
                tenant.ContactEmail,
                tenant.ContactPhone,
                tenant.Address,
                tenant.SupportEmail));
    }
}

public record SuspendTenantRequest(string Reason);
