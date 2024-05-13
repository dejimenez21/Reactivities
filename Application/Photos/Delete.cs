using Application.Core;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class Delete
{
    public record Command(string Id) : IRequest<Result<Unit>> { }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly AppDbContext _context;
        private readonly IUserContext _userContext;
        private readonly IPhotoAccessor _photoAccessor;

        public Handler(AppDbContext context, IUserContext userContext, IPhotoAccessor photoAccessor)
        {
            _context = context;
            _userContext = userContext;
            _photoAccessor = photoAccessor;
        }
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.Include(u => u.Photos).FirstOrDefaultAsync(x => x.UserName == _userContext.UserName) ?? throw new UserContextUserNotFoundException(_userContext.UserName);

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

            if (photo is null) return null!;

            if (photo.IsMain) return Result<Unit>.Failure("You cannot delete your main photo");

            var result = await _photoAccessor.DeletePhoto(photo.Id);

            if (!result) return Result<Unit>.Failure("Problem deleting photo from cloudinary");

            user.Photos.Remove(photo);

            var success = await _context.SaveChangesAsync() > 0;

            if (!success)
            {
                return Result<Unit>.Failure("Problem deleting photo from api");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
