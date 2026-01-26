using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Messages.Events.CaseStudies;

namespace SoftwareConsultingPlatform.Content.Api.Consumers;

public class CaseStudyPublishedEventConsumer : IConsumer<CaseStudyPublishedEvent>
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CaseStudyPublishedEventConsumer> _logger;

    public CaseStudyPublishedEventConsumer(IDistributedCache cache, ILogger<CaseStudyPublishedEventConsumer> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CaseStudyPublishedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Case study {CaseStudyId} published: {ProjectTitle} for tenant {TenantId}",
            message.CaseStudyId, message.ProjectTitle, message.TenantId);

        await _cache.RemoveAsync($"featured-casestudies:{message.TenantId}");
        await _cache.RemoveAsync($"casestudies:{message.TenantId}");
    }
}
