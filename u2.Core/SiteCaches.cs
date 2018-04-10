using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core
{
    public class SiteCaches : ISiteCaches
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private readonly IDictionary<Guid, ICache> _caches = new Dictionary<Guid, ICache>();

        private readonly ICacheRegistry _cacheRegistry;

        private readonly Func<string, ICacheStore> _getStore;

        public SiteCaches(ICacheRegistry cacheRegistry)
        {
            _cacheRegistry = cacheRegistry;
            _getStore = name => new CacheStore(name);
            var cacheStore = _getStore(null);
            Default = new Cache(cacheStore, _cacheRegistry);
        }

        public ICache Default { get; }

        public ICache Get(ISite site)
        {
            var key = site.Key;
            if (!_caches.TryGetValue(key, out var result))
            {
                try
                {
                    _semaphore.Wait();
                    if (!_caches.TryGetValue(key, out result))
                    {
                        var cacheStore = _getStore(site.SiteName);
                        result = _caches[key] = new Cache(cacheStore, _cacheRegistry, site);
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            return result;
        }

        public async Task RefreshAsync(ISite site = null)
        {
            if (site == null)
            {
                foreach (var cache in _caches.Values)
                {
                    await cache.ReloadAsync().ConfigureAwait(false);
                }
            }
            else
                await Get(site).ReloadAsync().ConfigureAwait(false);
        }

        public void Refresh(ISite site = null)
        {
            RefreshAsync(site).Wait();
        }
    }
}
