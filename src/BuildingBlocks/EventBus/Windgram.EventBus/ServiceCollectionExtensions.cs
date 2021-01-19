using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;

namespace Windgram.EventBus
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramEventBusInMemory(this IServiceCollection services, IConfiguration configuration, Assembly[] assemblies = null)
        {
            services.Configure<EventBusConfig>(configuration.GetSection(EventBusConfig.CONFIGURATION_KEY));
            var eventBusConfig = services.BuildServiceProvider().GetRequiredService<IOptions<EventBusConfig>>().Value;
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
