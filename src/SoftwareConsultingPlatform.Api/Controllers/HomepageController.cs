using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoftwareConsultingPlatform.Api.Features.Homepage.GetFeaturedCaseStudies;
using SoftwareConsultingPlatform.Api.Features.Homepage.GetFeaturedServices;
using SoftwareConsultingPlatform.Api.Features.Homepage.GetHomepageContent;

namespace SoftwareConsultingPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomepageController : ControllerBase
{
    private readonly IMediator _mediator;

    public HomepageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("content")]
    public async Task<IActionResult> GetContent(CancellationToken cancellationToken)
    {
        var content = await _mediator.Send(new GetHomepageContentQuery(), cancellationToken);

        if (content == null)
        {
            return NotFound();
        }

        return Ok(content);
    }

    [HttpGet("featured-case-studies")]
    public async Task<IActionResult> GetFeaturedCaseStudies(CancellationToken cancellationToken)
    {
        var featured = await _mediator.Send(new GetFeaturedCaseStudiesQuery(), cancellationToken);
        return Ok(featured);
    }

    [HttpGet("featured-services")]
    public async Task<IActionResult> GetFeaturedServices(CancellationToken cancellationToken)
    {
        var featured = await _mediator.Send(new GetFeaturedServicesQuery(), cancellationToken);
        return Ok(featured);
    }
}
