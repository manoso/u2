using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Caching
{
    public class Cache : ICache
    {
        private readonly ICacheStore _store;
        private readonly ICacheRegistry _registry;

        public Cache(ICacheStore store, ICacheRegistry registry)
        {
            _store = store;
            _registry = registry;
        }

        public async Task<IEnumerable<T>> FetchAsync<T>(string key = null)
        {
            var result = await DoFetch<T>(string.IsNullOrWhiteSpace(key) ? typeof(T).FullName : key);
            return result as IEnumerable<T>;
        }

        public async Task<ILookup<string, T>> FetchAsync<T>(ICacheLookup<T> lookup)
        {
            if (lookup == null)
                return null;

            var result = await DoFetch<T>(lookup.CacheKey, true);
            return result as ILookup<string, T>;
        }

        private async Task<object> DoFetch<T>(string key, bool isLookup = false)
        {
            var type = typeof(T);
            var taskKey = isLookup ? type.FullName : key;

            return _registry.TryGetTask(taskKey, out ICacheTask task)
                ? await TaskFetch(task, key)
                : null;
        }

        private async Task<object> TaskFetch(ICacheTask task, string cacheKey)
        {
            if (!_store.Has(cacheKey) || task.IsExpired)
                await task.Run((k, v) => _store.Save(k, v));

            return _store.Get(cacheKey);
        }
    }
}




