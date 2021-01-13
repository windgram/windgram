namespace Windgram.EventBus
{
    public interface IIntegrationEventConsumer<in TIntegrationEvent> : MassTransit.IConsumer<TIntegrationEvent>
      where TIntegrationEvent : IntegrationEvent
    {
    }
}
