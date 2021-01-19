using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Windgram.Caching
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramMemoryCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheConfig>(configuration.GetSection(CacheConfig.CONFIGURATION_KEY));
            services.AddMemoryCache();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
            return services;
        }
    }
}
