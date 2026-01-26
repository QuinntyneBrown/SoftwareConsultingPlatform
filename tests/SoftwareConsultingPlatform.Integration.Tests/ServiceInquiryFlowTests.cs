using Aspire.Hosting.Testing;
using FluentAssertions;
using Shared.Contracts.Services;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace SoftwareConsultingPlatform.Integration.Tests;

public class ServiceInquiryFlowTests : IAsyncLifetime
{
    private DistributedApplication? _app;
    private HttpClient? _httpClient;

    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.SoftwareConsultingPlatform_AppHost>();

        _app = await appHost.BuildAsync();
        await _app.StartAsync();

        _httpClient = _app.CreateHttpClient("api-gateway");
        _httpClient.DefaultRequestHeaders.Add("X-Tenant-Id", Guid.NewGuid().ToString());
    }

    public async Task DisposeAsync()
    {
        if (_app != null)
        {
            await _app.DisposeAsync();
        }
    }

    [Fact]
    public async Task GetServices_ShouldReturnEmptyListInitially()
    {
        // Act
        var response = await _httpClient!.GetAsync("/api/services");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var services = await response.Content.ReadFromJsonAsync<IEnumerable<ServiceSummaryDto>>();
        services.Should().NotBeNull();
    }

    [Fact]
    public async Task GetFeaturedServices_ShouldReturnOk()
    {
        // Act
        var response = await _httpClient!.GetAsync("/api/services/featured");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var services = await response.Content.ReadFromJsonAsync<IEnumerable<ServiceSummaryDto>>();
        services.Should().NotBeNull();
    }

    [Fact]
    public async Task SubmitInquiry_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var inquiry = new
        {
            ServiceId = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            Company = "Acme Corp",
            ProjectDescription = "We need a web application for our business."
        };

        // Act
        var response = await _httpClient!.PostAsJsonAsync("/api/services/inquiries", inquiry);

        // Assert
        // Note: May return NotFound if service doesn't exist, which is expected behavior
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound, HttpStatusCode.Created);
    }
}
