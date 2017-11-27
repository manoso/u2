using u2.Caching.Contract;

namespace u2.Test
{
    public class TestCacheConfig : ICacheConfig
    {
        public int CacheInSeconds { get; } = 3;
    }
}