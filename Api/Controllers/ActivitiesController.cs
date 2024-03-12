using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllActivitiesAsync(CancellationToken ct)
    {
        return HandleResult(await Mediator.Send(new List.Query(), ct));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivityById(Guid id)
    {
        var result = await Mediator.Send(new Details.Query { Id = id });
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostActivity(Activity activity)
    {
        return HandleResult(await Mediator.Send(new Create.Command { Activity = activity }));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditActivity(Guid id, Activity activity)
    {
        activity.Id = id;
        return HandleResult(await Mediator.Send(new Edit.Command { Activity = activity, ActivityId = id }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { ActivityId = id }));
    }

    [HttpPost("{id}/attend")]
    public async Task<IActionResult> Attend(Guid id)
    {
        return HandleResult(await Mediator.Send(new UpdateAttendance.Command { ActivityId = id }));
    }

}
