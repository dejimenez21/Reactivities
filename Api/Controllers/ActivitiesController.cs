using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllActivitiesAsync(CancellationToken ct)
    {
        return HandleResult(await Sender.Send(new List.Query(), ct));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivityById(Guid id)
    {
        var result = await Sender.Send(new Details.Query(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostActivity(Activity activity)
    {
        return HandleResult(await Sender.Send(new Create.Command(activity)));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditActivity(Guid id, Activity activity)
    {
        activity.Id = id;
        return HandleResult(await Sender.Send(new Edit.Command(activity, id)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
        return HandleResult(await Sender.Send(new Delete.Command(id)));
    }

    [HttpPost("{id}/attend")]
    public async Task<IActionResult> Attend(Guid id)
    {
        return HandleResult(await Sender.Send(new UpdateAttendance.Command(id)));
    }

}
