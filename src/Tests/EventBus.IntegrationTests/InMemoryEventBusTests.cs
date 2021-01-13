using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Xunit;

namespace Windgram.EventBus.IntegrationTests
{
    public class InMemoryEventBusTests
    {
        private readonly IEventBus _eventBus;
        public InMemoryEventBusTests()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddOptions();
            services.AddLogging();
            services.Configure<EventBusConfig>(config =>
            {
                config.QueueName = "EventBusTests";
                config.RetryCount = 5;
            });
            var eventBusConfig = services.BuildServiceProvider().GetRequiredService<IOptions<EventBusConfig>>().Value;
            var assembly = GetType().Assembly;
            services.AddMassTransit(config =>
            {
                config.AddConsumers(assembly);
                config.UsingInMemory((ctx, mq) =>
                {
                    mq.Host();

                    mq.ReceiveEndpoint(eventBusConfig.QueueName, x =>
                    {
                        x.ConfigureConsumers(ctx);
                    });
                });
            });
            services.AddMassTransitHostedService();
            services.AddSingleton<IEventBus, InMemoryEventBus>();
            _eventBus = services.BuildServiceProvider().GetRequiredService<IEventBus>();
        }

        [Fact]
        public async Task Can_Publish_Event()
        {
            await _eventBus.Publish(new TestIntegrationEvent("Hello world!"));
        }
    }
}
