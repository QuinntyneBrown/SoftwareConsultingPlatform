using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Messages.Events.CaseStudies;

namespace SoftwareConsultingPlatform.Content.Api.Consumers;

public class CaseStudyFeaturedChangedEventConsumer : IConsumer<CaseStudyFeaturedChangedEvent>
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CaseStudyFeaturedChangedEventConsumer> _logger;

    public CaseStudyFeaturedChangedEventConsumer(IDistributedCache cache, ILogger<CaseStudyFeaturedChangedEventConsumer> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CaseStudyFeaturedChangedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Case study {CaseStudyId} featured status changed to {IsFeatured} for tenant {TenantId}",
            message.CaseStudyId, message.IsFeatured, message.TenantId);

        await _cache.RemoveAsync($"featured-casestudies:{message.TenantId}");
        await _cache.RemoveAsync($"homepage:{message.TenantId}");
    }
}
