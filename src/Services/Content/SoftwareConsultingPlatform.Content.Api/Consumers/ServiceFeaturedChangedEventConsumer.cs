using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Messages.Events.Services;

namespace SoftwareConsultingPlatform.Content.Api.Consumers;

public class ServiceFeaturedChangedEventConsumer : IConsumer<ServiceFeaturedChangedEvent>
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<ServiceFeaturedChangedEventConsumer> _logger;

    public ServiceFeaturedChangedEventConsumer(IDistributedCache cache, ILogger<ServiceFeaturedChangedEventConsumer> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ServiceFeaturedChangedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Service {ServiceId} featured status changed to {IsFeatured} for tenant {TenantId}",
            message.ServiceId, message.IsFeatured, message.TenantId);

        await _cache.RemoveAsync($"featured-services:{message.TenantId}");
        await _cache.RemoveAsync($"homepage:{message.TenantId}");
    }
}
