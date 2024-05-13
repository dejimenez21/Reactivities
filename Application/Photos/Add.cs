using Application.Core;
using Application.Exceptions;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class Add
{
    public record Command(IFormFile File) : IRequest<Result<Photo>> { }

    public class Handler : IRequestHandler<Command, Result<Photo>>
    {
        private readonly AppDbContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserContext _userContext;

        public Handler(AppDbContext context, IPhotoAccessor photoAccessor, IUserContext userContext)
        {
            _context = context;
            _photoAccessor = photoAccessor;
            _userContext = userContext;
        }

        public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.Include(u => u.Photos).FirstOrDefaultAsync(x => x.UserName == _userContext.UserName) ?? throw new UserContextUserNotFoundException(_userContext.UserName);

            var result = await _photoAccessor.AddPhoto(request.File);
            if (!result.IsSuccess)
            {
                return Result<Photo>.Failure(result.Error!);
            }

            var photoUploadResult = result.Value!;

            var photo = new Photo
            {
                Url = photoUploadResult.Url,
                Id = photoUploadResult.PublicId
            };

            if (!user.Photos.Any(x => x.IsMain))
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            var success = await _context.SaveChangesAsync() > 0;

            if (!success)
            {
                return Result<Photo>.Failure("Problem adding photo");
            }

            return Result<Photo>.Success(photo);
        }
    }
}

