using MediatR;

namespace SoftwareConsultingPlatform.Api.Features.Services.SubmitInquiry;

public sealed class SubmitInquiryCommand : IRequest<SubmitInquiryResponse>
{
    public Guid ServiceId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string ProjectDescription { get; set; } = string.Empty;
}