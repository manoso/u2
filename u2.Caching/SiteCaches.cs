using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Caching
{
    public class SiteCaches : ISiteCaches
    {
        public static int DefaultCache = 300;

        private readonly IDictionary<string, ICache> _caches = new Dictionary<string, ICache>();

        public ICache this[string key]
        {
            get => _caches[key];
            set => _caches[key] = value;
        }

        public async Task RefreshAsync(string site = null)
        {
            if (string.IsNullOrWhiteSpace(site))
            {
                foreach (var cache in _caches.Values)
                {
                    await cache.ReloadAsync().ConfigureAwait(false);
                }
            }
            else
                await _caches[site].ReloadAsync().ConfigureAwait(false);
        }

        public void Refresh(string site = null)
        {
            RefreshAsync(site).Wait();
        }
    }
}
