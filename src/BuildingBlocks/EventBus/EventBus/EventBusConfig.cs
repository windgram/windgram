namespace Windgram.EventBus
{
    public class EventBusConfig
    {
        public const string CONFIGURATION_KEY = "EventBus";
        public string QueueName { get; set; }
        public int RetryCount { get; set; } = 5;
    }
}
