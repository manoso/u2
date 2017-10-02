using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Cache
{
    public class CacheFetcher : ICacheFetcher
    {
        private readonly ICacheStore _store;
        private readonly ICacheRegistry _registry;

        public CacheFetcher(ICacheStore store, ICacheRegistry registry)
        {
            _store = store;
            _registry = registry;
        }

        public async Task<IEnumerable<T>> FetchAsync<T>()
        {
            return await FetchAsync<IEnumerable<T>, T>(typeof(T).FullName);
        }

        public async Task<T> FetchAsync<T>(string key)
        {
            return await FetchAsync<T, T>(key);
        }

        public async Task<ILookup<string, T>> FetchLookupAsync<T>(ILookupParameter<T> lookupParameter)
        {
            if (lookupParameter == null)
                return null;

            return await FetchAsync<ILookup<string, T>, T>(lookupParameter.CacheKey, true);
        }

        private async Task<TResult> FetchAsync<TResult, T>(string key, bool isLookup = false)
        {
            var type = typeof(T);
            var taskKey = isLookup ? type.FullName : key;

            return _registry.TryGetTask(taskKey, out ICacheTask task)
                ? await TaskFetch<TResult>(task, key)
                : default(TResult);
        }

        private async Task<T> TaskFetch<T>(ICacheTask task, string cacheKey)
        {
            if (!_store.Has(cacheKey) || task.IsExpired)
            {
                await task.Run<T>();
                foreach (var cacheItem in task.CacheItems)
                    _store.Save(cacheItem.Key, cacheItem.Value);
            }

            return (T)_store.Get(cacheKey);
        }
    }
}




