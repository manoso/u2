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
    public static class Context
    {
        private static Func<IRoot> _rootFunc;
        private static Func<ICache> _cacheFunc;

        public static IMapRegistry MapRegistry { get; private set; }
        public static IMapper Mapper { get; private set; }
        public static ICacheRegistry CacheRegistry { get; private set; }
        public static IQueryFactory QueryFactory { get; private set; }
        public static ICmsFetcher CmsFetcher { get; private set; }
        public static IRegistry Registry { get; private set; }
        public static ISiteCaches SiteCaches { get; private set; }

        public static ICache Cache => _cacheFunc();
        public static IRoot Root => _rootFunc();

        public static void Init<TRoot>(ICacheConfig cacheConfig, IUmbracoConfig umbracoConfig, IMapBuild mapBuild, ICacheBuild cacheBuild)
            where TRoot : class, IRoot
        {
            MapRegistry = new MapRegistry();
            Mapper = new Mapper(MapRegistry);
            CacheRegistry = new CacheRegistry(cacheConfig);
            QueryFactory = new UmbracoQueryFactory();
            CmsFetcher = new UmbracoFetcher(umbracoConfig);
            Registry = new Registry(MapRegistry, Mapper, CacheRegistry, QueryFactory, CmsFetcher);
            SiteCaches = new SiteCaches(CacheRegistry);

            _rootFunc = () =>
            {
                var cache = SiteCaches.Default;
                var host = HttpContext.Current?.Request.Url.Host ?? string.Empty;
                var sites = cache.Fetch<TRoot>()?.ToList();
                return sites?.FirstOrDefault(site => site.Hosts?.Contains(host) ?? false) ?? sites?.FirstOrDefault();
            };

            _cacheFunc = () =>
            {
                var root = _rootFunc();
                return SiteCaches.Get(root) as Cache;
            };

            mapBuild.Setup(Registry);
            cacheBuild.Setup(CacheRegistry);
        }
    }
}
