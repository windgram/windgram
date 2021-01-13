using Windgram.EventBus;

namespace Windgram.Application.Shared.IntegrationEvents
{
    public class SendEmailIntegrationEvent : IntegrationEvent
    {
        public string From { get; set; }
        public string FromDisplayName { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Code { get; set; }
        public string IpAddress { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
