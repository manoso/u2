using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u2.Core.Contract;
using u2.Core.Extensions;

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

        public async Task<IEnumerable<T>> FetchAsync<T>(string key = null)
        {
            var result = await DoFetch<T>(string.IsNullOrWhiteSpace(key) ? typeof(T).FullName : key);
            return result.OfType<T>().AsList();
        }

        //public async Task<ILookup<string, T>> FetchLookupAsync<T>(ILookupParameter<T> lookupParameter)
        //{
        //    if (lookupParameter == null)
        //        return null;

        //    var result = await FetchAsync<ILookup<string, T>, T>(lookupParameter.CacheKey, true);
        //    return result.OfType<ILookup<string, T>>()
        //}

        private async Task<IEnumerable<object>> DoFetch<T>(string key, bool isLookup = false)
        {
            var type = typeof(T);
            var taskKey = isLookup ? type.FullName : key;

            return _registry.TryGetTask(taskKey, out ICacheTask task)
                ? await TaskFetch(task, key)
                : null;
        }

        private async Task<IEnumerable<object>> TaskFetch(ICacheTask task, string cacheKey)
        {
            if (!_store.Has(cacheKey) || task.IsExpired)
            {
                await task.Run();
                foreach (var cacheItem in task.CacheItems)
                    _store.Save(cacheItem.Key, cacheItem.Value);
            }

            return (IEnumerable<object>)_store.Get(cacheKey);
        }
    }
}




