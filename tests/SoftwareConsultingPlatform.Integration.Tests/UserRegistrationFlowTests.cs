using Aspire.Hosting.Testing;
using FluentAssertions;
using Shared.Contracts.Identity;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace SoftwareConsultingPlatform.Integration.Tests;

public class UserRegistrationFlowTests : IAsyncLifetime
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
    }

    public async Task DisposeAsync()
    {
        if (_app != null)
        {
            await _app.DisposeAsync();
        }
    }

    [Fact]
    public async Task Register_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var request = new
        {
            Email = $"test-{Guid.NewGuid()}@example.com",
            Password = "Password123!",
            FullName = "Integration Test User"
        };

        // Act
        var response = await _httpClient!.PostAsJsonAsync("/api/auth/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();
        result.Should().NotBeNull();
        result!.UserId.Should().NotBeEmpty();
        result.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Register_AndLogin_ShouldReturnTokens()
    {
        // Arrange
        var email = $"test-{Guid.NewGuid()}@example.com";
        var password = "Password123!";

        var registerRequest = new
        {
            Email = email,
            Password = password,
            FullName = "Login Test User"
        };

        // Register first
        var registerResponse = await _httpClient!.PostAsJsonAsync("/api/auth/register", registerRequest);
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act - Login
        var loginRequest = new { Email = email, Password = password };
        var loginResponse = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await loginResponse.Content.ReadFromJsonAsync<AuthTokenDto>();
        result.Should().NotBeNull();
        result!.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ShouldReturnConflict()
    {
        // Arrange
        var email = $"duplicate-{Guid.NewGuid()}@example.com";
        var request = new
        {
            Email = email,
            Password = "Password123!",
            FullName = "Duplicate Test User"
        };

        // First registration
        var firstResponse = await _httpClient!.PostAsJsonAsync("/api/auth/register", request);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act - Second registration with same email
        var secondResponse = await _httpClient.PostAsJsonAsync("/api/auth/register", request);

        // Assert
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}

public record RegisterResponseDto(Guid UserId, string Email);
