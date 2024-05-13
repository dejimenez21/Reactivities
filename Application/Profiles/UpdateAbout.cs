using Application.Core;
using Application.Exceptions;
using Application.IntegrationEvents.Users.Updated;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class UpdateAbout
{
    public record Command(string DisplayName, string? Bio) : IRequest<Result<Unit>> { }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserContext _userContext;
        private readonly IPublisher _publisher;

        public Handler(AppDbContext dbContext, IUserContext userContext, IPublisher publisher)
        {
            _dbContext = dbContext;
            _userContext = userContext;
            _publisher = publisher;
        }
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var appUser = await _dbContext.AppUsers.FirstOrDefaultAsync(x => x.UserName == _userContext.UserName) ?? throw new UserContextUserNotFoundException(_userContext.UserName);

            appUser.DisplayName = request.DisplayName;
            appUser.Bio = request.Bio;

            var success = await _dbContext.SaveChangesAsync() > 0;

            if (!success)
            {
                return Result<Unit>.Failure("Failed to update about");
            }

            await _publisher.Publish(new UserAboutUpdatedIntegrationEvent(appUser.Id, appUser.DisplayName, appUser.Bio));

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
