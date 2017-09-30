using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core
{
    public class DataPool : IDataPool
    {
        private readonly IMap _map;
        private readonly IMapRegistry _mapRegistry;
        private readonly ICacheRegistry _cacheRegistry;
        private readonly ICacheFetcher _cacheFetcher;
        private readonly IQueryFactory _queryFactory;
        private readonly ICmsFetcher _cmsFetcher;

        public DataPool(IMap map, IMapRegistry mapRegistry, ICacheRegistry cacheRegistry, ICacheFetcher cacheFetcher, IQueryFactory queryFactory, ICmsFetcher cmsFetcher)
        {
            _map = map;
            _mapRegistry = mapRegistry;
            _cacheRegistry = cacheRegistry;
            _cacheFetcher = cacheFetcher;
            _queryFactory = queryFactory;
            _cmsFetcher = cmsFetcher;
        }

        public async Task<IEnumerable<T>> GetAsync<T>()
            where T : class, new()
        {
            if (!_cacheRegistry.Has<T>())
                await Register<T>();

            return await _cacheFetcher.FetchAsync<T>();
        }

        public Task<T> GetAsync<T>(string key)
        {
            return _cacheRegistry.Has(key) ? _cacheFetcher.FetchAsync<T>(key) : null;
        }

        public Task<ILookup<string, T>> GetLookupAsync<T>(ILookupParameter<T> lookupParameter)
            where T : class, new()
        {
            return _cacheFetcher.FetchLookupAsync(lookupParameter);
        }

        private async Task Register<T>(string key = null) where T : class, new()
        {
            var typeMap = _mapRegistry.For<T>();
            var cmsQuery = _queryFactory.Create(typeMap);

            var defer = new MapDefer();
            await ModelDefer<T>(defer, typeMap);
            //if (!isMedia && !isRoot)
            //{
            //    MediaDefer<T>(defer, typeMap, root, medias);
            //}

            _cacheRegistry.Add(async () =>
            {
                var contents = await Task.Run(() => _cmsFetcher.Fetch(cmsQuery));
                var models = _map.To<T>(contents, defer);
                return models;
            },  key: key);
        }

        public async Task<IEnumerable<object>> GetModels(Type type)
        {
            return await _cacheFetcher.FetchAsync<IEnumerable<object>>(type.FullName);
        }

        public async Task<IEnumerable<T>> GetModels<T>()
        {
            return await _cacheFetcher.FetchAsync<T>();
        }

        private async Task ModelDefer<T>(MapDefer defer, TypeMap typeMap)
            where T : class, new()
        {
            var typeDefer = defer.For<T>();
            foreach (var modelMap in typeMap.ModelMaps)
            {
                var map = modelMap;
                var alias = map.Alias;
                var source = await GetModels(map.ModelType);
                typeDefer.Attach<string>(alias, (x, csv) =>
                {
                    if (string.IsNullOrWhiteSpace(csv) || x == null) return;
                    var ids = csv.Split(',');
                    map.Match(x, ids, source);
                });
            }
        }

        //public string ToUrl(IEnumerable<Media> medias, string mediaKey)
        //{
        //    var media = medias?.FirstOrDefault(x => x.Is(mediaKey));
        //    return media?.UmbracoFile.Url;
        //}

        //private void MediaDefer<T>(MapDefer defer, TypeMap typeMap, IRoot home, IEnumerable<Media> medias) where T : class, new()
        //{
        //    if (defer == null || typeMap.MediaMaps.IsEmpty() || medias == null)
        //        return;

        //    var typeDefer = defer.For<T>();
        //    var imageBase = home == null ? string.Empty : home.ImageBaseUrl;
        //    foreach (var map in typeMap.MediaMaps)
        //    {
        //        var info = map;
        //        var alias = info.ToAlias();
        //        typeDefer.Attach<string>(alias, (x, key) =>
        //        {
        //            if (string.IsNullOrWhiteSpace(key)) return;

        //            var url = ToUrl(medias, key);
        //            if (url == null) return;

        //            info.SetValue(x, imageBase + url);
        //        });
        //    }
        //}

        //private async Task<IEnumerable<T>> GetUmbraco<T>(Expression<Func<T, bool>> condition = null)
        //    where T : class, new()
        //{
        //    string query = null;
        //    if (condition != null)
        //    {
        //        var visitor = new ExamineVisitor();
        //        visitor.Visit(condition);
        //        query = visitor.Query;
        //    }

        //    var isMedia = typeof(T) == typeof(Media);
        //    var isRoot = typeof(IRoot).IsAssignableFrom(typeof(T));
        //    var root = isRoot ? null : _mapRegistry.Root;
        //    //var medias = isMedia || isRoot ? null : await GetModels<Media>();
        //    var config = _mapRegistry.For<T>();
        //    if (config == null)
        //        return null;

        //    var queryFinal = isMedia
        //        ? MediaFormat
        //        : string.Format(ContentFormat, root?.Id.ToString() ?? string.Empty, config.Alias,
        //            query == null ? string.Empty : $" +({query})");
        //    var contents = await Task.Run(() => ContentFor(queryFinal));
        //    var defer = new MapDefer();
        //    await ModelDefer<T>(defer, config);
        //    if (!isMedia && !isRoot)
        //    {
        //        MediaDefer<T>(defer, typeMap, root, medias);
        //    }
        //    return _map.To<T>(contents, defer).ToList();
        //}
    }
}
