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

        public IEnumerable<T> Fetch<T>(string key = null)
        {
            return FetchAsync<T>(key).Result;
        }

        public ILookup<string, T> Fetch<T>(ICacheLookup<T> lookup)
        {
            return FetchAsync(lookup).Result;
        }

        public async Task<IEnumerable<T>> FetchAsync<T>(string key = null)
        {
            var result = await DoFetchAsync<T>(string.IsNullOrWhiteSpace(key) ? typeof(T).FullName : key).ConfigureAwait(false);
            return result as IEnumerable<T>;
        }

        public async Task<ILookup<string, T>> FetchAsync<T>(ICacheLookup<T> lookup)
        {
            if (lookup == null)
                return null;

            var result = await DoFetchAsync<T>(lookup.CacheKey, true).ConfigureAwait(false);
            return result as ILookup<string, T>;
        }

        private async Task<object> DoFetchAsync<T>(string key, bool isLookup = false)
        {
            var type = typeof(T);
            var taskKey = isLookup ? type.FullName : key;

            return _registry.TryGetTask(taskKey, out ICacheTask task)
                ? await TaskFetchAsync(task, key).ConfigureAwait(false)
                : null;
        }

        private async Task<object> TaskFetchAsync(ICacheTask task, string cacheKey)
        {
            if (!_store.Has(cacheKey) || task.IsExpired)
                await task.Run((k, v) => _store.Save(k, v)).ConfigureAwait(false);

            return _store.Get(cacheKey);
        }
    }
}




