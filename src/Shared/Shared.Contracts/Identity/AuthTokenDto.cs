namespace Shared.Contracts.Identity;

public record AuthTokenDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt
);
