using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class SetMain
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Id { get; set; }
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
            var user = await _context.AppUsers.Include(u => u.Photos).FirstOrDefaultAsync(x => x.UserName == _userContext.UserName);
            if (user is null)
            {
                return null;
            }

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);
            if (photo is null)
            {
                return null;
            }

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain is not null)
            {
                currentMain.IsMain = false;
            }

            photo.IsMain = true;

            var success = await _context.SaveChangesAsync() > 0;

            if(!success)
            {
                return Result<Unit>.Failure("Problem setting main photo");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
