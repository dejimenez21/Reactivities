using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly AppDbContext _context;
            private readonly IUserContext _userContext;

            public Handler(AppDbContext context, IUserContext userContext)
            {
                _context = context;
                _userContext = userContext;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == _userContext.UserName);

                request.Activity.Attendees.Add(new ActivityAttendee
                {
                    AppUser = user,
                    IsHost = true
                });

                _context.Activities.Add(request.Activity);

                var result = await _context.SaveChangesAsync() > 0;

                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to create activity");
            }
        }
    }
}