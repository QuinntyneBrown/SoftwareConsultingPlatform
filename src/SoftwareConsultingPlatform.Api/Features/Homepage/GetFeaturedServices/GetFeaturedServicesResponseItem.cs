namespace SoftwareConsultingPlatform.Api.Features.Homepage.GetFeaturedServices;

public sealed class GetFeaturedServicesResponseItem
{
    public Guid ServiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Tagline { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
}