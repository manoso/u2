using System.Linq;
using u2.Caching;
using u2.Caching.Contract;
using u2.Core;
using u2.Core.Contract;
using u2.Fixture.Contract;
using u2.Umbraco;
using u2.Umbraco.Contract;

namespace u2.Fixture
{
    public class BindConfig
    {
        private readonly IBinder _binder;

        public BindConfig(IBinder binder)
        {
            _binder = binder;
        }

        public void Config<TRoot, TUmbracoConfig, TCacheConfig, TMapBuild, TCacheBuild>() 
            where TRoot : class, IRoot, new ()
            where TUmbracoConfig : class, IUmbracoConfig
            where TCacheConfig : class, ICacheConfig
            where TMapBuild : class, IMapBuild
            where TCacheBuild : class, ICacheBuild
        {
            _binder.Add<IMapRegistry, MapRegistry>(true);
            _binder.Add<IMapper, Mapper>(true);
            _binder.Add<ICacheRegistry, CacheRegistry>(true);
            _binder.Add<IQueryFactory, UmbracoQueryFactory>(true);
            _binder.Add<ICmsFetcher, UmbracoFetcher>(true);
            _binder.Add<IRegistry, Registry>(true);
            _binder.Add<ISiteCaches, SiteCaches>(true);
            _binder.Add<IUmbracoConfig, TUmbracoConfig>(true);
            _binder.Add<ICacheConfig, TCacheConfig>(true);
            _binder.Add<IMapBuild, TMapBuild>(true);
            _binder.Add<ICacheBuild, TCacheBuild>(true);

            var rego = _binder.Get<IRegistry>();
            var mapBuild = _binder.Get<IMapBuild>();
            mapBuild.Setup(rego);
            var cacheRego = _binder.Get<ICacheRegistry>();
            var cacheBuild = _binder.Get<ICacheBuild>();
            cacheBuild.Setup(cacheRego);
            var siteCaches = _binder.Get<ISiteCaches>();

            _binder.Add<IRoot, TRoot>(func: () =>
            {
                var cache = siteCaches.Default;
                var host = _binder.Host;
                var sites = cache.Fetch<TRoot>()?.ToList();
                var root = sites?.FirstOrDefault(site => site.Hosts?.Contains(host) ?? false) ?? sites?.FirstOrDefault();
                return root;
            });

            _binder.Add<ICache, Cache>(func: () =>
            {
                var root = _binder.Get<IRoot>();
                var cache = siteCaches.Get(root) as Cache;
                return cache;
            });
        }
    }
}
