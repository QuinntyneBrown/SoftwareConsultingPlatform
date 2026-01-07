using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoftwareConsultingPlatform.Api.Features.Homepage.GetFeaturedCaseStudies;

namespace SoftwareConsultingPlatform.Api.Controllers;

[ApiController]
[Route("api/case-studies")]
public class CaseStudiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CaseStudiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCaseStudies(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        CancellationToken cancellationToken = default)
    {
        _ = page;
        _ = pageSize;
        _ = cancellationToken;
        return StatusCode(StatusCodes.Status501NotImplemented, new { message = "Case studies listing is not implemented yet." });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCaseStudy(Guid id, CancellationToken cancellationToken)
    {
        _ = id;
        _ = cancellationToken;
        return StatusCode(StatusCodes.Status501NotImplemented, new { message = "Case study by id is not implemented yet." });
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<IActionResult> GetCaseStudyBySlug(string slug, CancellationToken cancellationToken)
    {
        _ = slug;
        _ = cancellationToken;
        return StatusCode(StatusCodes.Status501NotImplemented, new { message = "Case study by slug is not implemented yet." });
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeaturedCaseStudies(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetFeaturedCaseStudiesQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCaseStudies(
        [FromQuery] string q,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        CancellationToken cancellationToken = default)
    {
        _ = q;
        _ = page;
        _ = pageSize;
        _ = cancellationToken;
        return StatusCode(StatusCodes.Status501NotImplemented, new { message = "Case studies search is not implemented yet." });
    }
}
