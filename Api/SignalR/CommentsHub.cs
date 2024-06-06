using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Api.SignalR;

public class CommentsHub : Hub
{
    private readonly ISender _sender;

    public CommentsHub(ISender sender)
    {
        _sender = sender;
    }

    public async Task SendComment(Create.Command command)
    {
        var comment = await _sender.Send(command);
        if(comment.IsSuccess)
        {
            await Clients.Group(command.ActivityId.ToString()).SendAsync("ReceiveComment", comment.Value);
        }
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var activityId = httpContext!.Request.Query["activityId"].ToString();
        await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

        var query = new List.Query(Guid.Parse(activityId));
        var result = await _sender.Send(query);
        await Clients.Caller.SendAsync("LoadComments", result.Value);
    }
}
