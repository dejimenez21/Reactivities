using MediatR;

namespace Application.IntegrationEvents.Users.Updated;

public record UserAboutUpdatedIntegrationEvent(string UserId, string DisplayName, string? Bio) : INotification
{
}
