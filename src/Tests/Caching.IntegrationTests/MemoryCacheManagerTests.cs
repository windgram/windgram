using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Windgram.Caching.IntegrationTests
{
    public class MemoryCacheManagerTests
    {
        private readonly ICacheManager _cacheManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceCollection _serviceDescriptors;
        public MemoryCacheManagerTests()
        {
            _serviceDescriptors = new ServiceCollection();
            _serviceDescriptors.Configure<CacheConfig>(x =>
            {
                x.DefaultCacheTimeMinutes = 30;
            });
            _serviceDescriptors.AddMemoryCache();
            _serviceDescriptors.AddSingleton<ICacheManager, MemoryCacheManager>();

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
