using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDto?>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto?>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<ActivityDto?>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.ProjectTo<ActivityDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(a => a.Id == request.Id);
                
                return Result<ActivityDto?>.Success(activity);
            }
        }
    }
}