using u2.Core.Contract;

namespace u2.Test
{
    public class TestCacheConfig : ICacheConfig
    {
        public int CacheInSeconds { get; } = 3;
    }
}