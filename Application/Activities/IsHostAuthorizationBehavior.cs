using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class IsHostAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IHostOnlyActivityCommand<TResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly IUserContext _userContext;

    public IsHostAuthorizationBehavior(AppDbContext dbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var activity = await _dbContext.Activities
            .Include(a => a.Attendees.Where(x => x.AppUserId == _userContext.Id))
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.ActivityId, cancellationToken);

        if(activity is null)
        {
            return await next();
        }

        var attendee = activity.Attendees.FirstOrDefault();
        
        if (attendee is null || !attendee.IsHost)
        {
            throw new UnauthorizedAccessException($"User {_userContext.UserName} is not authorized to edit this activity ({request.ActivityId}).");
        }

        return await next();
    }
}
