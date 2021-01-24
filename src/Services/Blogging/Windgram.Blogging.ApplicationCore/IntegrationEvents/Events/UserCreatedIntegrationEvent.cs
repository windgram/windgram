using Windgram.EventBus;

namespace Windgram.Identity.ApplicationCore.IntegrationEvents.Events
{
    public class UserCreatedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }
    }
}
