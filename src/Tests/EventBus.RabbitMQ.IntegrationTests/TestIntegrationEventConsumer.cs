using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windgram.EventBus.RabbitMQ.IntegrationTests
{
    public class TestIntegrationEventConsumer : IIntegrationEventConsumer<TestIntegrationEvent>
    {
        public bool Handled { get; private set; }
        public TestIntegrationEventConsumer()
        {
            Handled = false;
        }
        public async Task Consume(ConsumeContext<TestIntegrationEvent> context)
        {
            this.Handled = true;
            await Task.FromResult(0);
        }
    }
}
