using Application.IntegrationEvents.Users.Updated;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Integration;

internal class UserAboutUpdatedIntegrationEventHandler : INotificationHandler<UserAboutUpdatedIntegrationEvent>
{
    private readonly IdentityContext _context;

    public UserAboutUpdatedIntegrationEventHandler(IdentityContext context)
    {
        _context = context;
    }
    public async Task Handle(UserAboutUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == notification.UserId);

            user!.DisplayName = notification.DisplayName;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new HandleIntegrationEventFailedException(nameof(UserAboutUpdatedIntegrationEvent), notification, ex);
        }
    }
}
