using u2.Core;
using u2.Core.Contract;
using u2.Umbraco.DataType;

namespace u2.Umbraco
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Examine;
    using Examine.SearchCriteria;

    public class UmbracoFactory : IModelFactory
    {
        private const string DataSearcher = "DataSearcher";

        private const string ContentFormat = @"+__IndexType:content +__Path:\-1,{0}* +__NodeTypeAlias:{1}{2}";
        private const string MediaFormat = "+__IndexType:media";

        private readonly IMap _map;
        private readonly IMapRegistry _mapRegistry;
        private readonly ICacheRegistry _cacheRegistry;
        private readonly ICacheFetcher _fetcher;

        public UmbracoFactory(IMap map, IMapRegistry mapRegistry, ICacheRegistry cacheRegistry, ICacheFetcher cacheFetcher)
        {
            _map = map;
            _mapRegistry = mapRegistry;
            _cacheRegistry = cacheRegistry;
            _fetcher = cacheFetcher;
        }

        public async Task<IEnumerable<Media>> GetMedia()
        {
            var contents = await Task.Run(() => ContentFor(MediaFormat));
            return _map.To<Media>(contents).ToList();
        }

        public async Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> condition = null)
            where T : class, new()
        {
            return await GetUmbraco(condition);
        }

        private async Task<IEnumerable<T>> GetUmbraco<T>(Expression<Func<T, bool>> condition = null)
            where T : class, new()
        { 
            string query = null;
            if (condition != null)
            {
                var visitor = new ExamineVisitor();
                visitor.Visit(condition);
                query = visitor.Query;
            }

            var isMedia = typeof(T) == typeof(Media);
            var isRoot = typeof(IRoot).IsAssignableFrom(typeof(T));
            var root = isRoot ? null : _mapRegistry.Root;
            //var medias = isMedia || isRoot ? null : await GetModels<Media>();
            var config = _mapRegistry.For<T>();
            if (config == null)
                return null;

            var queryFinal = isMedia
                ? MediaFormat
                : string.Format(ContentFormat, root?.Id.ToString() ?? string.Empty, config.Alias,
                    query == null ? string.Empty : $" +({query})");
            var contents = await Task.Run(() => ContentFor(queryFinal));
            var defer = new MapDefer();
            await ModelDefer<T>(defer, config);
            //if (!isMedia && !isRoot)
            //{
            //    MediaDefer<T>(defer, config, root, medias);
            //}
            return _map.To<T>(contents, defer).ToList();
        }

        private static IEnumerable<IContent> ContentFor(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            var searcher = ExamineManager.Instance.SearchProviderCollection[DataSearcher];
            var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
            searchCriteria.RawQuery(query);
            var results = searcher.Search(searchCriteria);
            return results.Select(x => new UmbracoContent(x.Fields)).ToList();
        }

        public async Task<IEnumerable<object>> GetModels(Type type)
        {
            return await _fetcher.FetchAsync<IEnumerable<object>>(type.FullName);
        }

        public async Task<IEnumerable<T>> GetModels<T>()
        {
            return await _fetcher.FetchAsync<T>();
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

        private async Task ModelDefer<T>(MapDefer defer, TypeMap config)
            where T : class, new()
        {
            var typeDefer = defer.For<T>();
            foreach (var modelMap in config.ModelMaps)
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

        //private IEnumerable<UmbracoContent> ChildrenFor(IContent parent)
        //{
        //    var searcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
        //    var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
        //    searchCriteria.RawQuery("__path:" + string.Format("{0}*", parent.Get<string>("__path")));
        //    var results = searcher.Search(searchCriteria);

        //    return results.Select(x => new UmbracoContent(x.Fields)).ToList();
        //}
    }
}
