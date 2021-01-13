namespace Windgram.EventBus.RabbitMQ.IntegrationTests
{
    public class TestIntegrationEvent : IntegrationEvent
    {
        public string Name { get; set; }
        public TestIntegrationEvent(string name)
        {
            Name = name;
        }
    }
}
