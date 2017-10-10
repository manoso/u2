using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;
using u2.Core.Extensions;

namespace u2.Core
{
    public class Registry : IRegistry
    {
        private readonly IMapRegistry _mapRegistry;
        private readonly IMapper _mapper;
        private readonly ICacheRegistry _cacheRegistry;
        private readonly ICacheFetcher _cacheFetcher;
        private readonly IQueryFactory _queryFactory;
        private readonly ICmsFetcher _cmsFetcher;

        public Registry(IMapRegistry mapRegistry, IMapper mapper, ICacheRegistry cacheRegistry, ICacheFetcher cacheFetcher, IQueryFactory queryFactory, ICmsFetcher cmsFetcher)
        {
            _mapRegistry = mapRegistry;
            _mapper = mapper;
            _cacheRegistry = cacheRegistry;
            _cacheFetcher = cacheFetcher;
            _queryFactory = queryFactory;
            _cmsFetcher = cmsFetcher;
        }

        public ITypeMap<T> Register<T>(string key = null)
            where T : class, new()
        {
            var type = typeof(T);
            var typeMap = _mapRegistry.Register<T>();

            if (string.IsNullOrWhiteSpace(key))
                key = type.FullName;

            var defer = ModelDefer(typeMap);

            _cacheRegistry.Add(async () =>
            {
                var cmsQuery = _queryFactory.Create(typeMap);
                var contents = _cmsFetcher.Fetch(cmsQuery);
                var models = (await _mapper.To<T>(contents, defer)).AsList();
                return models;
            }, key: key);

            return typeMap;
        }

        private async Task<IEnumerable<object>> DoGet(Type type, string key = null)
        {
            key = string.IsNullOrWhiteSpace(key) ? type.FullName : key;
            return await _cacheFetcher.FetchAsync<object>(key);
        }

        private MapDefer ModelDefer(ITypeMap typeMap)
        {
            var defer = new MapDefer();
            var typeDefer = defer.For(typeMap.EntityType);
            foreach (var modelMap in typeMap.ModelMaps)
            {
                var map = modelMap;
                var alias = map.Alias;
                typeDefer.Attach(alias, async (x, s) =>
                {
                    if (string.IsNullOrWhiteSpace(s) || x == null) return;

                    var source = await DoGet(map.ModelType);

                    if (modelMap.IsMany)
                        map.Match(x, s.Split(','), source);
                    else
                        map.Match(x, s, source);
                });
            }
            return defer;
        }
    }
}
