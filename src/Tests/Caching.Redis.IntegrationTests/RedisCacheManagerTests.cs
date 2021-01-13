using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Windgram.Caching.Redis.IntegrationTests
{
    public class RedisCacheManagerTests
    {
        private readonly ICacheManager _cacheManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceCollection _serviceDescriptors;
        public RedisCacheManagerTests()
        {
            _serviceDescriptors = new ServiceCollection();

            _serviceDescriptors.Configure<RedisCacheConfig>(config =>
            {
                config.ConnectionString = "localhost:6379,ssl=false";
                config.Database = 2;
                config.DefaultCacheTimeMinutes = 30;
            });

            _serviceDescriptors.AddSingleton(sp =>
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

            _serviceDescriptors.AddSingleton<ICacheManager, RedisCacheManager>();

            _serviceProvider = _serviceDescriptors.BuildServiceProvider();
            _cacheManager = _serviceProvider.GetRequiredService<ICacheManager>();
        }

        [Fact]
        public async Task Can_Set_And_Get_Object_From_Cache()
        {
            var cacheKey = "some_key_1";
            await _cacheManager.SetAsync(cacheKey, 1);
            var result = await _cacheManager.GetAsync<int>(cacheKey);

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task Can_Remove_Cache()
        {
            var cacheKey = "some_key_1";
            await _cacheManager.SetAsync(cacheKey, 1);
            await _cacheManager.RemoveAsync(cacheKey);
            var result = await _cacheManager.GetAsync<int>(cacheKey);

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task Can_Clear_Cache()
        {
            var cacheKey = "some_key_1";
            await _cacheManager.SetAsync(cacheKey, 1);
            await _cacheManager.ClearAsync();
            var result = await _cacheManager.GetAsync<int>(cacheKey);

            Assert.Equal(0, result);
        }
    }
}
