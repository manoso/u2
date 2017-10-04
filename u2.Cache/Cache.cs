using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Cache
{
    public class Cache : ICache
    {
        private readonly ICacheRegistry _registry;
        private readonly ICacheFetcher _fetcher;

        public Cache(ICacheRegistry registry, ICacheFetcher fetcher)
        {
            _registry = registry;
            _fetcher = fetcher;
        }

        public void Add<T>(Func<Task<IEnumerable<T>>> func, int cacheInSecs = 0, string key = null, params ILookupParameter<T>[] lookups)
        {
            _registry.Add(func, cacheInSecs, key, lookups);
        }

        public bool Has<T>()
        {
            return _registry.Has<T>();
        }

        public bool Has(string key)
        {
            return _registry.Has(key);
        }

        public bool TryGetTask(string taskKey, out ICacheTask task)
        {
            return _registry.TryGetTask(taskKey, out task);
        }

        public async Task Reload<T>(string key = null)
        {
            await _registry.Reload<T>(key);
        }

        public async Task Reload()
        {
            await _registry.Reload();
        }

        public async Task<IEnumerable<T>> FetchAsync<T>(string key = null)
        {
            return await _fetcher.FetchAsync<T>(key);
        }
    }
}
