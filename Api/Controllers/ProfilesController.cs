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
}
