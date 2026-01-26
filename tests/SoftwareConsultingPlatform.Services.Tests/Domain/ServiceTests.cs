using FluentAssertions;
using SoftwareConsultingPlatform.Services.Core.Aggregates;
using Xunit;

namespace SoftwareConsultingPlatform.Services.Tests.Domain;

public class ServiceTests
{
    [Fact]
    public void CreateService_WithValidData_ShouldCreateService()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var name = "Web Development";
        var slug = "web-development";
        var tagline = "Building modern web applications";
        var overview = "We provide comprehensive web development services.";
        var createdBy = "user123";

        // Act
        var service = new Service(tenantId, name, slug, tagline, overview, createdBy);

        // Assert
        service.ServiceId.Should().NotBeEmpty();
        service.TenantId.Should().Be(tenantId);
        service.Name.Should().Be(name);
        service.Slug.Should().Be(slug);
        service.Tagline.Should().Be(tagline);
        service.Overview.Should().Be(overview);
        service.Status.Should().Be("Draft");
        service.IsFeatured.Should().BeFalse();
    }

    [Fact]
    public void Publish_ShouldSetStatusToPublished()
    {
        // Arrange
        var service = CreateTestService();

        // Act
        service.Publish("user123");

        // Assert
        service.Status.Should().Be("Published");
        service.PublishedAt.Should().NotBeNull();
    }

    [Fact]
    public void Archive_ShouldSetStatusToArchived()
    {
        // Arrange
        var service = CreateTestService();
        service.Publish("user123");

        // Act
        service.Archive("user123");

        // Assert
        service.Status.Should().Be("Archived");
    }

    [Fact]
    public void SetFeatured_WithTrue_ShouldSetIsFeaturedToTrue()
    {
        // Arrange
        var service = CreateTestService();
        service.Publish("user123");

        // Act
        service.SetFeatured(true, 1);

        // Assert
        service.IsFeatured.Should().BeTrue();
        service.DisplayOrder.Should().Be(1);
    }

    [Fact]
    public void Update_ShouldUpdateServiceProperties()
    {
        // Arrange
        var service = CreateTestService();
        var newName = "Updated Service";
        var newTagline = "Updated tagline";
        var newOverview = "Updated overview";

        // Act
        service.Update(newName, service.Slug, newTagline, newOverview, "user123");

        // Assert
        service.Name.Should().Be(newName);
        service.Tagline.Should().Be(newTagline);
        service.Overview.Should().Be(newOverview);
        service.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    private static Service CreateTestService()
    {
        return new Service(
            Guid.NewGuid(),
            "Test Service",
            "test-service",
            "Test tagline",
            "Test overview",
            "user123");
    }
}
