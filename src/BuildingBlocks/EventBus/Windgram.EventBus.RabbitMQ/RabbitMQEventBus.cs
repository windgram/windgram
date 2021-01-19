using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Windgram.EventBus.RabbitMQ
{
    public class RabbitMQEventBus : IEventBus
    {
        private readonly ILogger _logger;
        private readonly IBus _bus;
        private readonly RabbitMQConfig _mqConfig;
        public RabbitMQEventBus(
            ILogger<RabbitMQEventBus> logger,
            IBus bus,
            IOptions<RabbitMQConfig> options)
        {
            _logger = logger;
            _bus = bus;
            _mqConfig = options.Value;
        }

        public async Task Publish(IntegrationEvent @event, CancellationToken cancellationToken = default)
        {
            var policy = Policy.Handle<BrokerUnreachableException>()
                 .Or<SocketException>()
                 .WaitAndRetry(_mqConfig.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                 {
                     _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                 });

            await policy.Execute(async () =>
             {
                 _logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);

                 await _bus.Publish(@event);
             });
        }
    }
}
