using System.Text.Json;

namespace Identity.Integration;

internal class HandleIntegrationEventFailedException : Exception
{
    public HandleIntegrationEventFailedException(string eventName, object eventData, Exception? innerException) : base(GetConstructedMessage(eventName, eventData), innerException)
    {
        
    }

    private static string GetConstructedMessage(string eventName, object eventData)
    {
        var eventDataJson = JsonSerializer.Serialize(eventData);
        return $"Handle integration event failed for event {eventName} with data: \n{eventDataJson}";
    }
}
