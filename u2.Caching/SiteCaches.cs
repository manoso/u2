using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core;
using u2.Core.Contract;

namespace u2.Caching
{
    public class SiteCaches : ISiteCaches
    {
        public static int DefaultCacheTime = 300;

        private readonly IDictionary<IRoot, ICache> _caches = new Dictionary<IRoot, ICache>();

        public ICache Default { get; }

        public SiteCaches(ICache cache)
        {
            Default = cache;
        }

        public ICache this[IRoot key]
        {
            get => _caches[key];
            set => _caches[key] = value;
        }

        public async Task RefreshAsync(IRoot root = null)
        {
            if (root == null)
            {
                foreach (var cache in _caches.Values)
                {
                    await cache.ReloadAsync().ConfigureAwait(false);
                }
            }
            else
                await _caches[root].ReloadAsync().ConfigureAwait(false);
        }

        public void Refresh(IRoot root = null)
        {
            RefreshAsync(root).Wait();
        }
    }
}
