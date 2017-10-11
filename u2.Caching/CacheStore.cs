using System;
using System.Runtime.Caching;
using u2.Core.Contract;

namespace u2.Caching
{
    public class CacheStore : ICacheStore
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;

        public object Get(string key)
        {
            return Cache[key];
        }

        public void Save(string key, object item)
        {
            Cache.Set(key,
                item,
                new CacheItemPolicy
                {
                    SlidingExpiration = TimeSpan.Zero,
                    Priority = CacheItemPriority.Default
                });
        }

        public void Clear(string key)
        {
            Cache.Remove(key);
        }

        public bool Has(string key)
        {
            return Cache.Contains(key);
        }
    }
}
