using SoftwareConsultingPlatform.Core.Models.TenantAggregate;
using SoftwareConsultingPlatform.Core.Models.TenantAggregate.Enums;

namespace SoftwareConsultingPlatform.Core.Tests.Models;

public class TenantTests
{
    [Fact]
    public void Constructor_ShouldInitializeTenant_WithCorrectValues()
    {
        // Arrange
        var name = "Test Consulting";
        var subdomain = "test";
        var email = "contact@test.com";

        // Act
        var tenant = new Tenant(name, subdomain, email);

        // Assert
        Assert.Equal(name, tenant.Name);
        Assert.Equal(subdomain, tenant.Subdomain);
        Assert.Equal(email, tenant.Email);
        Assert.Equal(TenantStatus.Pending, tenant.Status);
        Assert.NotEqual(Guid.Empty, tenant.TenantId);
        Assert.True(tenant.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Activate_ShouldSetStatusToActive()
    {
        // Arrange
        var tenant = new Tenant("Test", "test", "test@test.com");

        // Act
        tenant.Activate();

        // Assert
        Assert.Equal(TenantStatus.Active, tenant.Status);
        Assert.NotNull(tenant.ActivatedAt);
    }

    [Fact]
    public void UpdateBranding_ShouldUpdateBrandingProperties()
    {
        // Arrange
        var tenant = new Tenant("Test", "test", "test@test.com");
        var logoUrl = "https://example.com/logo.png";
        var primaryColor = "#FF0000";

        // Act
        tenant.UpdateBranding(logoUrl, null, primaryColor, null, null);

        // Assert
        Assert.Equal(logoUrl, tenant.LogoUrl);
        Assert.Equal(primaryColor, tenant.PrimaryColor);
    }
}
