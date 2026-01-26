using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using SoftwareConsultingPlatform.Identity.Api.Features.Auth.Register;
using SoftwareConsultingPlatform.Identity.Infrastructure.Data;
using Shared.Core.Interfaces;
using Shared.Messages.Commands;
using Shared.Messages.Events.Identity;
using Xunit;

namespace SoftwareConsultingPlatform.Identity.Tests.Handlers;

public class RegisterCommandHandlerTests
{
    private readonly IdentityDbContext _context;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly Mock<ITenantContext> _tenantContextMock;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new IdentityDbContext(options);
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _tenantContextMock = new Mock<ITenantContext>();
        _tenantContextMock.Setup(x => x.TenantId).Returns(Guid.NewGuid());

        _handler = new RegisterCommandHandler(
            _context,
            _publishEndpointMock.Object,
            _tenantContextMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCreateUser()
    {
        // Arrange
        var command = new RegisterCommand(
            "newuser@example.com",
            "Password123!",
            "New User",
            null,
            null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().NotBeEmpty();
        result.Email.Should().Be(command.Email);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == command.Email);
        user.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldPublishUserRegisteredEvent()
    {
        // Arrange
        var command = new RegisterCommand(
            "event@example.com",
            "Password123!",
            "Event User",
            null,
            null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _publishEndpointMock.Verify(
            x => x.Publish(
                It.Is<UserRegisteredEvent>(e => e.Email == command.Email),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldPublishSendWelcomeEmailCommand()
    {
        // Arrange
        var command = new RegisterCommand(
            "welcome@example.com",
            "Password123!",
            "Welcome User",
            null,
            null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _publishEndpointMock.Verify(
            x => x.Publish(
                It.Is<SendWelcomeEmailCommand>(e => e.Email == command.Email),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ShouldThrowException()
    {
        // Arrange
        var email = "duplicate@example.com";
        var firstCommand = new RegisterCommand(email, "Password123!", "First User", null, null);
        var secondCommand = new RegisterCommand(email, "Password456!", "Second User", null, null);

        await _handler.Handle(firstCommand, CancellationToken.None);

        // Act
        var act = async () => await _handler.Handle(secondCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
