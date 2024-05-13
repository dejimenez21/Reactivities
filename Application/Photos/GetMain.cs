using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class GetMain
{
    public record Query(string Username) : IRequest<string?> { }

    public class Handler : IRequestHandler<Query, string?>
    {
        private readonly AppDbContext _context;

        public Handler(AppDbContext context, IUserContext userContext)
        {
            _context = context;
        }

        public async Task<string?> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.UserName == request.Username);

            if (user is null)
            {
                return null;
            }

            return user.Photos.FirstOrDefault(x => x.IsMain)?.Url;
        }
    }
}
