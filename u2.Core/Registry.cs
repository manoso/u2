using u2.Core.Contract;
using u2.Core.Extensions;

namespace u2.Core
{
    public class Registry : IRegistry
    {
        private readonly IMapRegistry _mapRegistry;
        private readonly IMapper _mapper;
        private readonly ICacheRegistry _cacheRegistry;
        private readonly IQueryFactory _queryFactory;
        private readonly ICmsFetcher _cmsFetcher;

        public Registry(IMapRegistry mapRegistry, IMapper mapper, ICacheRegistry cacheRegistry, IQueryFactory queryFactory, ICmsFetcher cmsFetcher)
        {
            _mapRegistry = mapRegistry;
            _mapper = mapper;
            _cacheRegistry = cacheRegistry;
            _queryFactory = queryFactory;
            _cmsFetcher = cmsFetcher;
        }

        public IMapTask<T> Register<T>(string key = null)
            where T : class, new()
        {
            var type = typeof(T);
            if (_mapRegistry.Has(type))
                return _mapRegistry[type] as IMapTask<T>;

            var mapTask = _mapRegistry.Register<T>();

            if (string.IsNullOrWhiteSpace(key))
                key = type.FullName;

            _cacheRegistry.Add(async x =>
            {
                var cache = x;
                var cmsQuery = _queryFactory.Create(cache.Site, mapTask);
                var contents = _cmsFetcher.Fetch(cmsQuery);
                var models = (await _mapper.ToAsync<T>(cache, contents).ConfigureAwait(false)).AsList();

                return models;
            }, key);

            return mapTask;
        }
    }
}
