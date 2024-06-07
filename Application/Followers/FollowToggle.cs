using Application.Core;
using Application.Exceptions;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers;

public class FollowToggle
{
    public record Command(string TargetUsername) : IRequest<Result<Unit>>
    {
    }

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
            var observer = await _dbContext.AppUsers.FirstOrDefaultAsync(x => x.UserName == _userContext.UserName) ?? throw new UserContextUserNotFoundException(_userContext.UserName);

            var target = await _dbContext.AppUsers.FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);
            if (target is null)
                return null!;

            var following = await _dbContext.UserFollowings.FindAsync(observer.Id, target.Id);

            if (following is null)
            {
                following = new UserFollowing
                {
                    Observer = observer,
                    Target = target
                };

                _dbContext.UserFollowings.Add(following);
            }
            else
            {
                _dbContext.UserFollowings.Remove(following);
            }

            var success = await _dbContext.SaveChangesAsync() > 0;

            if (!success) return Result<Unit>.Failure("Failed to update following");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
