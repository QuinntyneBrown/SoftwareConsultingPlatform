using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Identity.Core.Aggregates;
using SoftwareConsultingPlatform.Identity.Infrastructure.Data;
using Shared.Core.Interfaces;
using Shared.Messages.Commands;
using Shared.Messages.Events.Identity;

namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IdentityDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IdentityDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<RegisterCommandHandler> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registering new user with email: {Email}", request.Email);

        // Check if email already exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.TenantId == _tenantContext.TenantId && u.Email == request.Email, cancellationToken);

        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with this email already exists");
        }

        // Hash password using BCrypt
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Create user
        var user = new User(
            _tenantContext.TenantId,
            request.Email,
            passwordHash,
            request.FullName);

        if (!string.IsNullOrWhiteSpace(request.Phone) || !string.IsNullOrWhiteSpace(request.CompanyName))
        {
            user.UpdateProfile(request.FullName, request.Phone, request.CompanyName);
        }

        // Generate verification token
        var verificationToken = Guid.NewGuid().ToString("N");
        user.SetEmailVerificationToken(verificationToken);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish UserRegisteredEvent
        await _publishEndpoint.Publish(new UserRegisteredEvent(
            user.UserId,
            user.TenantId,
            user.Email,
            user.FullName,
            DateTime.UtcNow
        ), cancellationToken);

        // Publish command to send welcome email
        var verificationUrl = $"https://app.example.com/verify-email?token={verificationToken}";
        await _publishEndpoint.Publish(new SendWelcomeEmailCommand(
            user.TenantId,
            user.Email,
            user.FullName,
            verificationUrl
        ), cancellationToken);

        _logger.LogInformation("User registered successfully with ID: {UserId}", user.UserId);

        return new RegisterResponse
        {
            UserId = user.UserId,
            Email = user.Email,
            Message = "Registration successful. Please check your email to verify your account."
        };
    }
}
