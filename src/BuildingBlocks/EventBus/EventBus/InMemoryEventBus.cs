using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Windgram.EventBus
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly ILogger _logger;
        private readonly IBus _bus;
        private readonly EventBusConfig _busConfig;
        public InMemoryEventBus(
            ILogger<InMemoryEventBus> logger,
            IBus bus,
            IOptions<EventBusConfig> options)
        {
            _logger = logger;
            _bus = bus;
            _busConfig = options.Value;
        }

        public async Task Publish(IntegrationEvent @event, CancellationToken cancellationToken = default)
        {
            var policy = Policy.Handle<Exception>() 
                 .WaitAndRetry(_busConfig.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                 {
                     _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                 });

            await policy.Execute(async () =>
            {
                _logger.LogTrace("Publishing event to Memory: {EventId}", @event.Id);

                await _bus.Publish(@event);
            });
        }
    }
}
