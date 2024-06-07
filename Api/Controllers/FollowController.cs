using Application.Followers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class FollowController : BaseApiController
{
    [HttpPost("{username}")]
    public async Task<IActionResult> Follow(string username)
    {
        return HandleResult(await Sender.Send(new FollowToggle.Command(username)));
    }

}
