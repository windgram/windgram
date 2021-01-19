using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Windgram.EventBus
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramEventBusInMemory(this IServiceCollection services, IConfiguration configuration, Assembly[] assemblies = null)
        {
            var eventBusConfig = configuration.GetSection(EventBusConfig.CONFIGURATION_KEY).Get<EventBusConfig>();
            services.Configure<EventBusConfig>(options => options = eventBusConfig);

            var anyAssemblies = assemblies != null && assemblies.Any();
            services.AddMassTransit(config =>
            {
                if (anyAssemblies)
                {
                    config.AddConsumers(assemblies);
                }
                config.UsingInMemory((ctx, mq) =>
                {
                    mq.Host();

                    if (anyAssemblies)
                    {
                        mq.ReceiveEndpoint(eventBusConfig.QueueName, x =>
                        {
                            x.ConfigureConsumers(ctx);
                        });
                    }
                });
            });
            services.AddMassTransitHostedService();
            services.AddSingleton<IEventBus, InMemoryEventBus>();
            return services;
        }
    }
}
