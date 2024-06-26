﻿using MediatR;

namespace Application.IntegrationEvents.Users.Created;

public record UserCreatedIntegrationEvent(string Id, string UserName, string Email, string DisplayName) : INotification
{
}
