using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SoftwareConsultingPlatform.Services.Api.Consumers;
using SoftwareConsultingPlatform.Services.Core.Aggregates;
using SoftwareConsultingPlatform.Services.Infrastructure.Data;
using Shared.Messages.Events.Tenant;
using Xunit;

namespace SoftwareConsultingPlatform.Services.Tests.Consumers;

public class TenantSuspendedEventConsumerTests
{
    [Fact]
    public async Task Consume_ShouldArchiveAllServicesForTenant()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var options = new DbContextOptionsBuilder<ServicesDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new ServicesDbContext(options);

        // Create some services for the tenant
        var service1 = new Service(tenantId, "Service 1", "service-1", "Tag 1", "Overview 1", "user1");
        var service2 = new Service(tenantId, "Service 2", "service-2", "Tag 2", "Overview 2", "user1");
        service1.Publish("user1");
        service2.Publish("user1");

        context.Services.AddRange(service1, service2);
        await context.SaveChangesAsync();

        var loggerMock = new Mock<ILogger<TenantSuspendedEventConsumer>>();
        var consumer = new TenantSuspendedEventConsumer(context, loggerMock.Object);

        var contextMock = new Mock<ConsumeContext<TenantSuspendedEvent>>();
        contextMock.Setup(x => x.Message).Returns(new TenantSuspendedEvent(
            tenantId, "Test suspension", DateTime.UtcNow));

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        var services = await context.Services.Where(s => s.TenantId == tenantId).ToListAsync();
        services.Should().HaveCount(2);
        services.All(s => s.Status == "Archived").Should().BeTrue();
    }

    [Fact]
    public async Task Consume_WithNoServicesForTenant_ShouldNotThrow()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var options = new DbContextOptionsBuilder<ServicesDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new ServicesDbContext(options);

        var loggerMock = new Mock<ILogger<TenantSuspendedEventConsumer>>();
        var consumer = new TenantSuspendedEventConsumer(context, loggerMock.Object);

        var contextMock = new Mock<ConsumeContext<TenantSuspendedEvent>>();
        contextMock.Setup(x => x.Message).Returns(new TenantSuspendedEvent(
            tenantId, "Test suspension", DateTime.UtcNow));

        // Act
        var act = async () => await consumer.Consume(contextMock.Object);

        // Assert
        await act.Should().NotThrowAsync();
    }
}
