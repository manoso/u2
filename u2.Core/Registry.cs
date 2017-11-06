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
        private readonly ICache _cache;
        private readonly IQueryFactory _queryFactory;
        private readonly ICmsFetcher _cmsFetcher;

        public Registry(IMapRegistry mapRegistry, IMapper mapper, ICacheRegistry cacheRegistry, ICache cache, IQueryFactory queryFactory, ICmsFetcher cmsFetcher)
        {
            _mapRegistry = mapRegistry;
            _mapper = mapper;
            _cacheRegistry = cacheRegistry;
            _cache = cache;
            _queryFactory = queryFactory;
            _cmsFetcher = cmsFetcher;
        }

        public IMapTask<T> Register<T>(string key = null)
            where T : class, new()
        {
            var type = typeof(T);
            var typeMap = _mapRegistry.Register<T>();

            if (string.IsNullOrWhiteSpace(key))
                key = type.FullName;

            var mapDefer = new MapDefer();

            _cacheRegistry.Add(async () =>
            {
                mapDefer.Defer(typeMap, DoGet);
                var cmsQuery = _queryFactory.Create(typeMap);
                var contents = _cmsFetcher.Fetch(cmsQuery);
                var models = (await _mapper.To<T>(contents, mapDefer)).AsList();

                return models;
            }, key);

            return typeMap;
        }

        private async Task<IEnumerable<object>> DoGet(Type type, string key = null)
        {
            key = string.IsNullOrWhiteSpace(key) ? type.FullName : key;
            return await _cache.FetchAsync<object>(key).ConfigureAwait(false);
        }
    }
}
