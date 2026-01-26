using MassTransit;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Services.Infrastructure.Data;
using Shared.Messages.Events.Tenant;

namespace SoftwareConsultingPlatform.Services.Api.Consumers;

public class TenantSuspendedEventConsumer : IConsumer<TenantSuspendedEvent>
{
    private readonly ServicesDbContext _context;
    private readonly ILogger<TenantSuspendedEventConsumer> _logger;

    public TenantSuspendedEventConsumer(
        ServicesDbContext context,
        ILogger<TenantSuspendedEventConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TenantSuspendedEvent> context)
    {
        _logger.LogInformation("Processing TenantSuspendedEvent for tenant: {TenantId}", context.Message.TenantId);

        var services = await _context.Services
            .Where(s => s.TenantId == context.Message.TenantId)
            .ToListAsync();

        foreach (var service in services)
        {
            service.Archive();
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Archived {Count} services for suspended tenant: {TenantId}",
            services.Count, context.Message.TenantId);
    }
}
