using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Windgram.Caching.Redis
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisCacheConfig>(configuration.GetSection(CacheConfig.CONFIGURATION_KEY));

            services.AddSingleton(sp =>
            {
                var redisConfig = sp.GetRequiredService<IOptions<RedisCacheConfig>>().Value;

                var configuration = ConfigurationOptions.Parse(redisConfig.ConnectionString, true);
                configuration.ResolveDns = true;
                if (redisConfig.Database.HasValue)
                {
                    configuration.DefaultDatabase = redisConfig.Database.Value;
                }
                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddSingleton<ICacheManager, RedisCacheManager>();
            return services;
        }
    }
}
