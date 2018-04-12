using System;
using System.Linq;
using System.Web;
using u2.Core;
using u2.Core.Contract;
using u2.Umbraco.Contract;

namespace u2.Umbraco
{
    /// <summary>
    /// Static u2 context. Use it if no IoC, otherwise use IoC to inject the instances.
    /// </summary>
    public static class UmbracoContext
    {
        private static Func<ISite> _siteFunc;
        private static Func<ICache> _cacheFunc;

        /// <summary>
        /// Get the map registry instance.
        /// </summary>
        public static IMapRegistry MapRegistry { get; private set; }

        /// <summary>
        /// Get the mapper instance.
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// Get the cache registry instance.
        /// </summary>
        public static ICacheRegistry CacheRegistry { get; private set; }

        /// <summary>
        /// Get the query factory instance.
        /// </summary>
        public static IQueryFactory QueryFactory { get; private set; }

        /// <summary>
        /// Get the cms fetcher instance.
        /// </summary>
        public static ICmsFetcher CmsFetcher { get; private set; }

        /// <summary>
        /// Get the registry instance.
        /// </summary>
        public static IRegistry Registry { get; private set; }

        /// <summary>
        /// Get the site caches instance.
        /// </summary>
        public static ISiteCaches SiteCaches { get; private set; }

        /// <summary>
        /// Get the cache for the current context.
        /// </summary>
        public static ICache Cache => _cacheFunc();

        /// <summary>
        /// Get the root for the current context.
        /// </summary>
        public static ISite Site => _siteFunc();

        /// <summary>
        /// Call once to initialize the context.
        /// </summary>
        /// <typeparam name="TSite">The CMS site model type.</typeparam>
        /// <param name="cacheConfig">The cache config instance.</param>
        /// <param name="umbracoConfig">The umbraco config instance.</param>
        /// <param name="mapBuild">The map build instance.</param>
        /// <param name="cacheBuild">The cache build instance.</param>
        public static void Init<TSite>(ICacheConfig cacheConfig, IUmbracoConfig umbracoConfig, IMapBuild mapBuild, ICacheBuild cacheBuild)
            where TSite : class, ISite
        {
            MapRegistry = new MapRegistry();
            Mapper = new Mapper(MapRegistry);
            CacheRegistry = new CacheRegistry(cacheConfig);
            QueryFactory = new UmbracoQueryFactory();
            CmsFetcher = new UmbracoFetcher(umbracoConfig);
            Registry = new Registry(MapRegistry, Mapper, CacheRegistry, QueryFactory, CmsFetcher);
            SiteCaches = new SiteCaches(CacheRegistry);

            _siteFunc = () =>
            {
                var cache = SiteCaches.Default;
                var host = HttpContext.Current?.Request.Url.Host ?? string.Empty;
                var sites = cache.Fetch<TSite>()?.ToList();
                return sites?.FirstOrDefault(site => site.Hosts?.Contains(host) ?? false) ?? sites?.FirstOrDefault();
            };

            _cacheFunc = () =>
            {
                var site = _siteFunc();
                return SiteCaches.Get(site) as Cache;
            };

            mapBuild.Setup(Registry);
            cacheBuild.Setup(CacheRegistry);
        }
    }
}
