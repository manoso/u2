using u2.Caching.Contract;

namespace u2.Demo.Api.Ninject
{
    public class CacheConfig : ICacheConfig
    {
        public int CacheInSeconds { get; } = 0;
    }
}