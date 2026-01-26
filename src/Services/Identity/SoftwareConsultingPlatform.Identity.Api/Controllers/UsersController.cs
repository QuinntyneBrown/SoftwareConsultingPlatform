using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareConsultingPlatform.Identity.Api.Features.Users.GetCurrentUser;
using SoftwareConsultingPlatform.Identity.Api.Features.Users.UpdateProfile;
using SoftwareConsultingPlatform.Identity.Api.Features.Users.ChangePassword;

namespace SoftwareConsultingPlatform.Identity.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("me")]
    public async Task<ActionResult<GetCurrentUserResponse>> GetCurrentUser()
    {
        var result = await _mediator.Send(new GetCurrentUserQuery());
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPut("me")]
    public async Task<ActionResult<UpdateProfileResponse>> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPut("me/password")]
    public async Task<ActionResult<ChangePasswordResponse>> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}
