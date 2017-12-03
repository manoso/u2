using System;
using System.Runtime.Caching;
using u2.Core.Contract;

namespace u2.Caching
{
    public class CacheStore : ICacheStore
    {
        private readonly MemoryCache _cache;

        public CacheStore(string name = null)
        {
            _cache = string.IsNullOrWhiteSpace(name) ? MemoryCache.Default : new MemoryCache(name);
        }

        public object Get(string key)
        {
            return _cache[key];
        }

        public void Save(string key, object item)
        {
            _cache.Set(key,
                item,
                new CacheItemPolicy
                {
                    SlidingExpiration = TimeSpan.Zero,
                    Priority = CacheItemPriority.Default
                });
        }

        public void Clear(string key)
        {
            _cache.Remove(key);
        }

        public bool Has(string key)
        {
            return _cache.Contains(key);
        }
    }
}
