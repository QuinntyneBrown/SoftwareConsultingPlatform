using FluentAssertions;
using Microsoft.Playwright;
using Xunit;

namespace SoftwareConsultingPlatform.E2E.Tests;

public class HomePageTests : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;

    private const string BaseUrl = "http://localhost:4200"; // Angular dev server

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null) await _page.CloseAsync();
        if (_context != null) await _context.DisposeAsync();
        if (_browser != null) await _browser.DisposeAsync();
        _playwright?.Dispose();
    }

    [Fact]
    public async Task HomePage_ShouldLoadSuccessfully()
    {
        // Act
        var response = await _page!.GotoAsync(BaseUrl);

        // Assert
        response.Should().NotBeNull();
        response!.Ok.Should().BeTrue();
    }

    [Fact]
    public async Task HomePage_ShouldDisplayHeroSection()
    {
        // Act
        await _page!.GotoAsync(BaseUrl);

        // Assert
        var heroSection = await _page.QuerySelectorAsync("[data-testid='hero-section']");
        heroSection.Should().NotBeNull();
    }

    [Fact]
    public async Task HomePage_ShouldDisplayServicesSection()
    {
        // Act
        await _page!.GotoAsync(BaseUrl);

        // Assert
        var servicesSection = await _page.QuerySelectorAsync("[data-testid='services-section']");
        servicesSection.Should().NotBeNull();
    }

    [Fact]
    public async Task Navigation_ShouldNavigateToServicesPage()
    {
        // Arrange
        await _page!.GotoAsync(BaseUrl);

        // Act
        await _page.ClickAsync("[data-testid='nav-services']");
        await _page.WaitForURLAsync($"{BaseUrl}/services");

        // Assert
        var url = _page.Url;
        url.Should().Contain("/services");
    }

    [Fact]
    public async Task Navigation_ShouldNavigateToCaseStudiesPage()
    {
        // Arrange
        await _page!.GotoAsync(BaseUrl);

        // Act
        await _page.ClickAsync("[data-testid='nav-case-studies']");
        await _page.WaitForURLAsync($"{BaseUrl}/case-studies");

        // Assert
        var url = _page.Url;
        url.Should().Contain("/case-studies");
    }
}
