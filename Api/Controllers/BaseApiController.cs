#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private ISender _sender;

    protected ISender Sender => _sender ??= 
        HttpContext.RequestServices.GetRequiredService<ISender>();

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if(result == null)
            return NotFound();
        if(result.IsSuccess && result.Value != null)
            return Ok(result.Value);
        if(result.IsSuccess && result.Value == null)
            return NotFound();
        return BadRequest(result.Error);
    }
}
