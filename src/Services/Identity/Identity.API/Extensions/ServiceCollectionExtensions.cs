using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windgram.ApplicationCore.Domain.Entities;
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
            services
                .AddWindgramCorsPolicy(configuration["CorsPolicy"])
                .AddWindgramMvc()
                .AddWindgramSwagger(apiName, configuration["ApiVersion"])
                .AddWindgramIdentityDbContexts(
                configuration["IdentityConnectionStrings"],
                configuration["ConfigurationConnectionStrings"],
                configuration["PersistedGrantConnectionStrings"], new Version(configuration["MySqlVersion"]))
                .AddWindgramIdentityApplication(configuration["IdentityConnectionStrings"])
                .AddWindgramRedisCache(configuration)
                .AddWindgramAuth(apiName, configuration["IdentityUrl"])
                .AddWindgramHttpUserContext()
                .AddWindgramEventBusRabbitMQ(configuration, typeof(UserIdentity).Assembly);
            return services;
        }
    }
}
