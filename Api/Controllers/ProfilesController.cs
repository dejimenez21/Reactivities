using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ProfilesController : BaseApiController
{
    [HttpGet("{username}")]
    public async Task<IActionResult> GetProfile(string username)
    {
        return HandleResult(await Sender.Send(new Details.Query(username)));
    }

    [HttpPut("about")]
    public async Task<IActionResult> UpdateProfile(UpdateAbout.Command command)
    {
        return HandleResult(await Sender.Send(command));
    }
}
