using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Xunit;

namespace Windgram.EventBus.RabbitMQ.IntegrationTests
{
    public class RabbitMQEventBusTests
    {
        private readonly IEventBus _eventBus;
        public RabbitMQEventBusTests()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddOptions();
            services.AddLogging();
            services.Configure<RabbitMQConfig>(config =>
            {
                config.RetryCount = 5;
                config.QueueName = "EventBusTests";
                config.HostName = "127.0.0.1";
                config.VirtualHost = "/";
                config.UserName = "admin";
                config.Password = "admin";
            });
            var mqConfig = services.BuildServiceProvider().GetRequiredService<IOptions<RabbitMQConfig>>().Value;
            var assembly = GetType().Assembly;
            services.AddMassTransit(config =>
            {
                config.AddConsumers(assembly);
                config.UsingRabbitMq((ctx, mq) =>
                {
                    mq.Host(mqConfig.HostName, mqConfig.VirtualHost, h =>
                    {
                        h.Username(mqConfig.UserName);
                        h.Password(mqConfig.Password);
                    });

                    mq.ReceiveEndpoint(mqConfig.QueueName, x =>
                    {
                        x.ConfigureConsumers(ctx);
                    });
                });
            });
            services.AddMassTransitHostedService();
            services.AddSingleton<IEventBus, RabbitMQEventBus>();
            _eventBus = services.BuildServiceProvider().GetRequiredService<IEventBus>();
        }

        [Fact]
        public async Task Can_Publish_Event()
        {
            await _eventBus.Publish(new TestIntegrationEvent("Hello world!"));
        }
    }
}
