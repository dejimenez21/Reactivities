using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.IntegrationEvents.Users.Created;

public class UserCreatedIntegrationEventHandler : INotificationHandler<UserCreatedIntegrationEvent>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserCreatedIntegrationEventHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Handle(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var appUser = _mapper.Map<AppUser>(notification);

        _context.AppUsers.Add(appUser);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
