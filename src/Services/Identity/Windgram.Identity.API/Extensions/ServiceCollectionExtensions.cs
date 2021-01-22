using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windgram.Identity.ApplicationCore.Domain.Entities;
using Windgram.Caching.Redis;
using Windgram.EventBus.RabbitMQ;
using Windgram.Identity.ApplicationCore;
using Windgram.Identity.Infrastructure;

namespace Windgram.Identity.API.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramIdentityAPI(this IServiceCollection services, IConfiguration configuration)
        {
            var apiName = configuration["ApiName"];
            var identityConnection = configuration.GetConnectionString("IdentityConnection");
            var configurationConnection = configuration.GetConnectionString("ConfigurationConnection");
            var persistedGrantConnection = configuration.GetConnectionString("PersistedGrantConnection");

            services
                .AddWindgramCorsPolicy(configuration["CorsPolicy"])
                .AddWindgramMvc()
                .AddWindgramSwagger(apiName, configuration["ApiVersion"])
                .AddWindgramIdentityDbContexts(identityConnection, configurationConnection, persistedGrantConnection, new Version(configuration["MySqlVersion"]))
                .AddWindgramIdentityApplication(identityConnection)
                .AddWindgramRedisCache(configuration)
                .AddWindgramAuth(apiName, configuration["IdentityUrl"])
                .AddWindgramHttpUserContext()
                .AddWindgramEventBusRabbitMQ(configuration, typeof(UserIdentity).Assembly);
            return services;
        }
    }
}
