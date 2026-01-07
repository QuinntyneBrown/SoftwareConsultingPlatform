using MediatR;
using SoftwareConsultingPlatform.Core;
using SoftwareConsultingPlatform.Core.Models.UserAggregate;

namespace SoftwareConsultingPlatform.Api.Features.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly ISoftwareConsultingPlatformContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        ISoftwareConsultingPlatformContext context,
        ITenantContext tenantContext,
        ILogger<RegisterCommandHandler> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registering new user with email: {Email}", request.Email);

        // Hash password using BCrypt
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Create user
        var user = new User(
            _tenantContext.TenantId,
            request.Email,
            passwordHash,
            request.FullName);

        if (!string.IsNullOrWhiteSpace(request.CompanyName))
        {
            user.UpdateProfile(request.FullName, null, request.CompanyName);
        }

        // Generate verification token
        var verificationToken = Guid.NewGuid().ToString("N");
        user.SetEmailVerificationToken(verificationToken);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User registered successfully with ID: {UserId}", user.UserId);

        return new RegisterResponse
        {
            UserId = user.UserId,
            Email = user.Email,
            Message = "Registration successful. Please check your email to verify your account."
        };
    }
}
