using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareConsultingPlatform.Core;
using System.Security.Claims;

namespace SoftwareConsultingPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ISoftwareConsultingPlatformContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        ISoftwareConsultingPlatformContext context,
        ITenantContext tenantContext,
        ILogger<UsersController> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var user = await _context.Users
            .Where(u => u.UserId == userId && u.TenantId == _tenantContext.TenantId)
            .Select(u => new
            {
                u.UserId,
                u.Email,
                u.FullName,
                u.Phone,
                u.CompanyName,
                u.EmailVerified,
                u.MfaEnabled,
                u.AvatarUrl,
                u.LastLoginAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateCurrentUser(
        [FromBody] UpdateProfileRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId && u.TenantId == _tenantContext.TenantId, cancellationToken);

        if (user == null)
            return NotFound();

        user.UpdateProfile(request.FullName, request.Phone, request.CompanyName);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User profile updated: {UserId}", userId);

        return Ok(new { message = "Profile updated successfully" });
    }

    [HttpPut("me/password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId && u.TenantId == _tenantContext.TenantId, cancellationToken);

        if (user == null)
            return NotFound();

        // Verify current password
        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
        {
            return BadRequest(new { message = "Current password is incorrect" });
        }

        // Update to new password
        var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatePassword(newPasswordHash);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User password changed: {UserId}", userId);

        return Ok(new { message = "Password changed successfully" });
    }
}

public class UpdateProfileRequest
{
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
