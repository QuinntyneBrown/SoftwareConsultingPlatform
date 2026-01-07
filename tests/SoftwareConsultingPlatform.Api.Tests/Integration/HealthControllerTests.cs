using System.Net;
using System.Net.Http.Json;

namespace SoftwareConsultingPlatform.Api.Tests.Integration;

public class HealthControllerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HealthControllerTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/health");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var health = await response.Content.ReadFromJsonAsync<HealthResponse>();
        Assert.NotNull(health);
        Assert.Equal("Healthy", health.Status);
    }

    private class HealthResponse
    {
        public string Status { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Service { get; set; } = string.Empty;
    }
}
