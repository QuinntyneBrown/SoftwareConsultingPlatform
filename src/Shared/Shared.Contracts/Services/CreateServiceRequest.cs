namespace Shared.Contracts.Services;

public record CreateServiceRequest(
    string Name,
    string? Tagline,
    string? Overview,
    string? IconUrl
);
