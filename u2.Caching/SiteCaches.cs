using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Caching
{
    public class SiteCaches
    {
        public static int DefaultCacheTime = 300;

        private static readonly IDictionary<IRoot, ICache> Caches = new Dictionary<IRoot, ICache>();

        public static ICache Default { get; set; }

        private static ICacheRegistry _cacheRegistry;

        public static void Setup(ICacheRegistry cacheRegistry, ICacheStore cacheStore)
        {
            _cacheRegistry = cacheRegistry;
            Default = new Cache(cacheStore, _cacheRegistry);
        }

        public static ICache Get(IRoot root)
        {
            return Caches[root];
        }

        public static void Add(IRoot root, ICache cache)
        {
            Caches[root] = cache;
        }

        public static bool Has(IRoot root)
        {
            return Caches.ContainsKey(root);
        }

        public static async Task RefreshAsync(IRoot root = null)
        {
            if (root == null)
            {
                foreach (var cache in Caches.Values)
                {
                    await cache.ReloadAsync().ConfigureAwait(false);
                }
            }
            else
                await Caches[root].ReloadAsync().ConfigureAwait(false);
        }

        public static void Refresh(IRoot root = null)
        {
            RefreshAsync(root).Wait();
        }
    }
}
