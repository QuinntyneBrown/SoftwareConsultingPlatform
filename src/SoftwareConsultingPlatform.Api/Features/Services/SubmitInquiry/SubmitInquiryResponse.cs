namespace SoftwareConsultingPlatform.Api.Features.Services.SubmitInquiry;

public sealed class SubmitInquiryResponse
{
    public string Message { get; set; } = "Inquiry submitted successfully";
    public Guid InquiryId { get; set; }
}