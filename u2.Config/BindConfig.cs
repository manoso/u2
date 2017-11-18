using System.Linq;
using u2.Caching;
using u2.Config.Contract;
using u2.Core;
using u2.Core.Contract;
using u2.Core.Extensions;
using u2.Umbraco;

namespace u2.Config
{
    public class BindConfig
    {
        private readonly IBinder _binder;

        public BindConfig(IBinder binder)
        {
            _binder = binder;
        }

        public void Config<TRoot, TConfig>() 
            where TRoot : class, IRoot, new ()
            where TConfig: class, IMapConfig
        {
            _binder.Add<IMapRegistry, MapRegistry>(true);
            _binder.Add<IMapper, Mapper>(true);
            _binder.Add<ICacheRegistry, CacheRegistry>(true);
            _binder.Add<IQueryFactory, UmbracoQueryFactory>(true);
            _binder.Add<ICmsFetcher, UmbracoFetcher>(true);
            _binder.Add<IRegistry, Registry>(true);
            _binder.Add<ICacheStore, CacheStore>();
            _binder.Add<IMapConfig, TConfig>(true);

            var rego = _binder.Get<IRegistry>();
            var config = _binder.Get<IMapConfig>();
            config.Config(rego);

            _binder.Add<IRoot, TRoot>(func: () =>
            {
                var host = _binder.Host;
                var cacheStore = _binder.Get<ICacheStore>();
                var cacheRegistry = _binder.Get<ICacheRegistry>();
                SiteCaches.Setup(cacheRegistry, cacheStore);
                var cache = SiteCaches.Default;
                var sites = cache.Fetch<TRoot>().AsList();
                return sites.FirstOrDefault(/*site => site.Hosts.Contains(host)*/);
            });

            _binder.Add<ICache, Cache>(func: () =>
            {
                var root = _binder.Get<IRoot>();
                if (!SiteCaches.Has(root))
                {
                    var cacheRego = _binder.Get<ICacheRegistry>();
                    var cacheStore = _binder.Get<ICacheStore>();
                    SiteCaches.Add(root, new Cache(cacheStore, cacheRego, root));
                }
                return SiteCaches.Get(root) as Cache;
            });
        }
    }
}
