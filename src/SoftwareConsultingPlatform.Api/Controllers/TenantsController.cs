using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;

namespace SoftwareConsultingPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly ISoftwareConsultingPlatformContext _context;
    private readonly ILogger<TenantsController> _logger;

    public TenantsController(
        ISoftwareConsultingPlatformContext context,
        ILogger<TenantsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetTenants(CancellationToken cancellationToken)
    {
        var tenants = await _context.Tenants
            .Select(t => new
            {
                t.TenantId,
                t.Name,
                t.Subdomain,
                t.Status,
                t.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Ok(tenants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTenant(Guid id, CancellationToken cancellationToken)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.TenantId == id, cancellationToken);

        if (tenant == null)
            return NotFound();

        return Ok(tenant);
    }
}
