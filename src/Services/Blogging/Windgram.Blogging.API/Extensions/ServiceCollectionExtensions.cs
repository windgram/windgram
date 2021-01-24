using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windgram.Blogging.ApplicationCore;
using Windgram.Blogging.ApplicationCore.Domain.Entities;
using Windgram.Blogging.Infrastructure;
using Windgram.Caching.Redis;
using Windgram.EventBus.RabbitMQ;

namespace Windgram.Blogging
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramBloggingAPI(this IServiceCollection services, IConfiguration configuration)
        {
            var apiName = configuration["ApiName"];

            services
                .AddWindgramCorsPolicy(configuration["CorsPolicy"])
                .AddWindgramMvc()
                .AddWindgramSwagger(apiName, configuration["ApiVersion"])
                .AddWindgramBloggingDbContexts(configuration.GetConnectionString("BloggingConnection"), new Version(configuration["MySqlVersion"]))
                .AddWindgramBloggingApplication(configuration.GetConnectionString("BloggingQueryConnection"))
                .AddWindgramRedisCache(configuration)
                .AddWindgramAuth(apiName, configuration["IdentityUrl"])
                .AddWindgramHttpUserContext()
                .AddWindgramEventBusRabbitMQ(configuration, typeof(Post).Assembly);
            return services;
        }
    }
}
