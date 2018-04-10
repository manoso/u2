using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core
{
    public class Cache : ICache
    {
        private readonly ICacheStore _store;
        private readonly ICacheRegistry _registry;

        public Cache(ICacheStore store, ICacheRegistry registry, ISite site = null)
        {
            _store = store;
            _registry = registry;

            Site = site;
        }

        public ISite Site { get; }

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

        public async Task ReloadAsync<T>(string key = null)
        {
            await _registry.ReloadAsync<T>(this, key).ConfigureAwait(false);
        }

        public async Task ReloadAsync()
        {
            await _registry.ReloadAsync(this).ConfigureAwait(false);
        }

        public void Reload<T>(string key = null)
        {
            ReloadAsync<T>(key).Wait();
        }

        public void Reload()
        {
            ReloadAsync().Wait();
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
            if (!_store.Has(cacheKey) || task.IsExpired(this))
                await task.Run(this, (k, v) => _store.Save(k, v)).ConfigureAwait(false);

            return _store.Get(cacheKey);
        }
    }
}




