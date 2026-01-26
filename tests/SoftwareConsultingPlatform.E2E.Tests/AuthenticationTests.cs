using FluentAssertions;
using Microsoft.Playwright;
using Xunit;

namespace SoftwareConsultingPlatform.E2E.Tests;

public class AuthenticationTests : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;

    private const string BaseUrl = "http://localhost:4200";

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
    public async Task LoginPage_ShouldLoadSuccessfully()
    {
        // Act
        var response = await _page!.GotoAsync($"{BaseUrl}/login");

        // Assert
        response.Should().NotBeNull();
        response!.Ok.Should().BeTrue();
    }

    [Fact]
    public async Task LoginPage_ShouldDisplayLoginForm()
    {
        // Act
        await _page!.GotoAsync($"{BaseUrl}/login");

        // Assert
        var emailInput = await _page.QuerySelectorAsync("[data-testid='email-input']");
        var passwordInput = await _page.QuerySelectorAsync("[data-testid='password-input']");
        var submitButton = await _page.QuerySelectorAsync("[data-testid='login-button']");

        emailInput.Should().NotBeNull();
        passwordInput.Should().NotBeNull();
        submitButton.Should().NotBeNull();
    }

    [Fact]
    public async Task LoginPage_WithInvalidCredentials_ShouldShowError()
    {
        // Arrange
        await _page!.GotoAsync($"{BaseUrl}/login");

        // Act
        await _page.FillAsync("[data-testid='email-input']", "invalid@example.com");
        await _page.FillAsync("[data-testid='password-input']", "wrongpassword");
        await _page.ClickAsync("[data-testid='login-button']");

        // Wait for error message
        await _page.WaitForSelectorAsync("[data-testid='error-message']", new PageWaitForSelectorOptions
        {
            Timeout = 5000
        });

        // Assert
        var errorMessage = await _page.QuerySelectorAsync("[data-testid='error-message']");
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task RegisterPage_ShouldLoadSuccessfully()
    {
        // Act
        var response = await _page!.GotoAsync($"{BaseUrl}/register");

        // Assert
        response.Should().NotBeNull();
        response!.Ok.Should().BeTrue();
    }

    [Fact]
    public async Task RegisterPage_ShouldDisplayRegistrationForm()
    {
        // Act
        await _page!.GotoAsync($"{BaseUrl}/register");

        // Assert
        var fullNameInput = await _page.QuerySelectorAsync("[data-testid='fullname-input']");
        var emailInput = await _page.QuerySelectorAsync("[data-testid='email-input']");
        var passwordInput = await _page.QuerySelectorAsync("[data-testid='password-input']");
        var submitButton = await _page.QuerySelectorAsync("[data-testid='register-button']");

        fullNameInput.Should().NotBeNull();
        emailInput.Should().NotBeNull();
        passwordInput.Should().NotBeNull();
        submitButton.Should().NotBeNull();
    }

    [Fact]
    public async Task LoginPage_ShouldHaveLinkToRegister()
    {
        // Arrange
        await _page!.GotoAsync($"{BaseUrl}/login");

        // Act
        var registerLink = await _page.QuerySelectorAsync("[data-testid='register-link']");

        // Assert
        registerLink.Should().NotBeNull();
    }
}
