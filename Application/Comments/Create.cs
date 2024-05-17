using Application.Core;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments;

public class Create
{
    public record Command(string Body, Guid ActivityId) : IRequest<Result<CommentDto>> { }

    public class CommentCreateCommandValidator : AbstractValidator<Command>
    {
        public CommentCreateCommandValidator()
        {
            RuleFor(c => c.Body).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<CommentDto>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public Handler(AppDbContext dbContext, IMapper mapper, IUserContext userContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _dbContext.Activities.FindAsync(request.ActivityId);
            if (activity == null) return null!;

            var user = await _dbContext.AppUsers
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == _userContext.UserName) ?? throw new UserContextUserNotFoundException(_userContext.UserName);

            var comment = new Comment
            {
                Body = request.Body,
                Author = user
            };

            activity.Comments.Add(comment);
            var success = await _dbContext.SaveChangesAsync() > 0;
            if(!success)
            {
                return Result<CommentDto>.Failure("Failed to add comment");
            }

            return Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
        }
    }
}
