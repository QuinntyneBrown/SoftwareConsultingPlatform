namespace Shared.Contracts.Identity;

public record UserDto(
    Guid UserId,
    string Email,
    string FullName,
    string? Phone,
    string? CompanyName,
    string? AvatarUrl,
    bool EmailVerified,
    bool MfaEnabled,
    string Status
);
