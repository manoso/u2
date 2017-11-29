using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using u2.Caching;
using u2.Caching.Contract;
using u2.Core;
using u2.Core.Contract;
using u2.Fixture.Contract;
using u2.Umbraco;
using u2.Umbraco.Contract;

namespace u2.Fixture
{
    public static class Resolver
    {
        private static IDictionary<Type, object> _singletons;
        private static Func<IRoot> _rootFunc;
        private static Func<ICache> _cacheFunc;

        public static void Init<TRoot>(ICacheConfig cacheConfig, IUmbracoConfig umbracoConfig, IMapBuild mapBuild, ICacheBuild cacheBuild)
            where TRoot: class, IRoot
        {
            var mapRegistry = new MapRegistry();
            var mapper = new Mapper(mapRegistry);
            var cacheRegistry = new CacheRegistry(cacheConfig);
            var queryFactory = new UmbracoQueryFactory();
            var umbracoFetcher = new UmbracoFetcher(umbracoConfig);
            var registry = new Registry(mapRegistry, mapper, cacheRegistry, queryFactory, umbracoFetcher);

            _singletons = new Dictionary<Type, object>
            {
                {typeof(IMapRegistry), mapRegistry },
                {typeof(IMapper), mapper },
                {typeof(ICacheRegistry), cacheRegistry },
                {typeof(IQueryFactory), queryFactory },
                {typeof(ICmsFetcher), umbracoFetcher },
                {typeof(IRegistry), registry },
            };

            _rootFunc = () =>
            {
                var cache = SiteCaches.Default;
                if (cache == null)
                {
                    var cacheStore = new CacheStore();
                    SiteCaches.Setup(cacheRegistry, cacheStore);
                    cache = SiteCaches.Default;
                }
                var host = HttpContext.Current?.Request.Url.Host ?? string.Empty;
                var sites = cache.Fetch<TRoot>()?.ToList();
                return sites?.FirstOrDefault(site => site.Hosts?.Contains(host) ?? false) ?? sites?.FirstOrDefault();
            };

            _cacheFunc = () =>
            {
                var root = _rootFunc();
                if (!SiteCaches.Has(root))
                {
                    var cacheStore = new CacheStore();
                    SiteCaches.Add(root, new Cache(cacheStore, cacheRegistry, root));
                }
                return SiteCaches.Get(root) as Cache;
            };

            mapBuild.Setup(registry);
            cacheBuild.Setup(cacheRegistry);
        }

        public static T Get<T>() 
            where T : class
        {
            var type = typeof(T);
            return (
                type == typeof(ICache)
                ? _cacheFunc()
                : type == typeof(IRoot)
                    ? _rootFunc()
                    : _singletons.TryGetValue(type, out var value)
                        ? value
                        : null
            ) as T;
        }
    }
}
