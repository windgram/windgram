namespace Windgram.Caching
{
    public class CacheConfig
    {
        public const string CONFIGURATION_KEY = "Caching";
        public int DefaultCacheTimeMinutes { get; set; } = 30;
    }
}
