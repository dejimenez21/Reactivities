using Application.Core;
using Application.Exceptions;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class UpdateAttendance
{
    public record Command(Guid ActivityId) : IRequest<Result<Unit>> { }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserContext _userContext;

        public Handler(AppDbContext dbContext, IUserContext userContext)
        {
            _dbContext = dbContext;
            _userContext = userContext;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _dbContext.Activities
                .Include(a => a.Attendees)
                .ThenInclude(a => a.AppUser)
                .FirstOrDefaultAsync(a => a.Id == request.ActivityId);

            if (activity is null) return null!;

            var user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.UserName == _userContext.UserName, cancellationToken) ?? throw new UserContextUserNotFoundException(_userContext.UserName);

            var attendance = activity.Attendees.FirstOrDefault(a => a.AppUserId == user.Id);

            if (attendance is not null && attendance.IsHost)
            {
                activity.IsCanceled = !activity.IsCanceled;
            }
            else if (attendance is not null && !attendance.IsHost)
            {
                activity.Attendees.Remove(attendance);
            }
            else
            {
                attendance = new ActivityAttendee
                {
                    AppUser = user,
                    IsHost = false
                };

                activity.Attendees.Add(attendance);
            }

            var result = await _dbContext.SaveChangesAsync() > 0;

            return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating attendance");
        }
    }
}
