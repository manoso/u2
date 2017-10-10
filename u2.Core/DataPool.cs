using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u2.Core.Contract;
using u2.Core.Extensions;

namespace u2.Core
{
    public class DataPool : IDataPool
    {
        private readonly IMapRegistry _mapRegistry;
        private readonly IMapper _mapper;
        private readonly ICacheRegistry _cacheRegistry;
        private readonly ICacheFetcher _cacheFetcher;
        private readonly IQueryFactory _queryFactory;
        private readonly ICmsFetcher _cmsFetcher;

        public DataPool(IMapRegistry mapRegistry, IMapper mapper, ICacheRegistry cacheRegistry, ICacheFetcher cacheFetcher, IQueryFactory queryFactory, ICmsFetcher cmsFetcher)
        {
            _mapRegistry = mapRegistry;
            _mapper = mapper;
            _cacheRegistry = cacheRegistry;
            _cacheFetcher = cacheFetcher;
            _queryFactory = queryFactory;
            _cmsFetcher = cmsFetcher;
        }

        public async Task<IEnumerable<T>> GetAsync<T>(string key = null)
            where T : class, new()
        {
            key = string.IsNullOrWhiteSpace(key) ? typeof(T).FullName : key;
            var result = await _cacheFetcher.FetchAsync<object>(key);
            return result.OfType<T>().AsList();
        }

        //public Task<ILookup<string, T>> GetLookupAsync<T>(ILookupParameter<T> lookupParameter)
        //    where T : class, new()
        //{
        //    return _cacheFetcher.FetchLookupAsync(lookupParameter);
        //}

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
        //    //var medias = isMedia || isRoot ? null : await Get<Media>();
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
