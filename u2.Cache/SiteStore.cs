using System;
using System.Runtime.Caching;

namespace u2.Cache
{
    public interface ISiteStore
    {
        object Get(string key);
        void Save(string key, object item);
        void Clear(string key);
        bool Has(string key);
    }

    public class SiteStore : ISiteStore
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
