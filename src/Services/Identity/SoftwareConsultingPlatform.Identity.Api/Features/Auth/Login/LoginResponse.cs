namespace SoftwareConsultingPlatform.Identity.Api.Features.Auth.Login;

public record LoginResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public UserInfo User { get; init; } = null!;
}

public record UserInfo
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? AvatarUrl { get; init; }
    public bool EmailVerified { get; init; }
    public bool MfaEnabled { get; init; }
}
