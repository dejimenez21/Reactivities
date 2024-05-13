using MediatR;

namespace Application.Activities;

public interface IHostOnlyActivityCommand<T> : IRequest<T>
{
    public Guid ActivityId { get; init; }
}
