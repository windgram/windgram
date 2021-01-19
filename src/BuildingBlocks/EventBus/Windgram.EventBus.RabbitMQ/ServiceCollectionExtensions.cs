using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;

namespace Windgram.EventBus.RabbitMQ
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramEventBusRabbitMQ(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            services.Configure<RabbitMQConfig>(configuration.GetSection(EventBusConfig.CONFIGURATION_KEY));
            var mqConfig = services.BuildServiceProvider().GetRequiredService<IOptions<RabbitMQConfig>>().Value;
            var anyAssemblies = assemblies != null && assemblies.Any();
            services.AddMassTransit(config =>
            {
                if (anyAssemblies)
                {
                    config.AddConsumers(assemblies);
                }
                config.UsingRabbitMq((ctx, mq) =>
                {
                    mq.Host(mqConfig.HostName, mqConfig.VirtualHost, h =>
                    {
                        h.Username(mqConfig.UserName);
                        h.Password(mqConfig.Password);
                    });

                    if (anyAssemblies)
                    {
                        mq.ReceiveEndpoint(mqConfig.QueueName, x =>
                        {
                            x.ConfigureConsumers(ctx);
                        });
                    }
                });
            });
            services.AddMassTransitHostedService();
            services.AddSingleton<IEventBus, RabbitMQEventBus>();
            return services;
        }
    }
}
