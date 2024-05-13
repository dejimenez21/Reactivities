using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class Create
{
    public record Command(Activity Activity) : IRequest<Result<ActivityDto>>
    {
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<ActivityDto>>
    {
        private readonly AppDbContext _context;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public Handler(AppDbContext context, IUserContext userContext, IMapper mapper)
        {
            _context = context;
            _userContext = userContext;
            _mapper = mapper;
        }
        public async Task<Result<ActivityDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == _userContext.UserName);

            request.Activity.Attendees.Add(new ActivityAttendee
            {
                AppUser = user!,
                IsHost = true
            });

            _context.Activities.Add(request.Activity);

            var result = await _context.SaveChangesAsync() > 0;

            return result ? Result<ActivityDto>.Success(_mapper.Map<ActivityDto>(request.Activity)) : Result<ActivityDto>.Failure("Failed to create activity");
        }
    }
}