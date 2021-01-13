namespace Windgram.Caching.Redis
{
    public class RedisCacheConfig : CacheConfig
    {
        public string ConnectionString { get; set; }
        public int? Database { get; set; }
    }
}
