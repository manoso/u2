//namespace Cinema.Data.Umbraco
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Linq.Expressions;
//    using System.Threading.Tasks;
//    using Cms.DataType;
//    using Cms.Mapping;
//    using Examine;
//    using Examine.SearchCriteria;
//    using Infrastructure.Cache;
//    using Infrastructure.Configuration;
//    using Model.Cms;
//    using Model.Cms.Section;
//    using Ninject;

//    public interface IUmbracoProvider : IContentProvider
//    {
//        Task<IEnumerable<Media>> GetMedia();
//        Task<Home> GetHome();
//    }

//    public class UmbracoProvider : IUmbracoProvider
//    {
//        private const string DataSearcher = "DataSearcher";

//        private const string ContentFormat = @"+__IndexType:content +__Path:\-1,{0}* +__NodeTypeAlias:{1}{2}";
//        private const string MediaFormat = "+__IndexType:media";

//        [Inject]
//        public ICinemaConfiguration Config { get; set; }

//        [Inject]
//        public ICacheHandler Cache { get; set; }

//        public async Task<IEnumerable<Media>> GetMedia()
//        {
//            var contents = await Task.Run(() => ContentFor(MediaFormat));
//            return contents.Solve<Media>().ToList();
//        }

//        public async Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> condition = null)
//            where T : class, ICmsModel, new()
//        {
//            string query = null;
//            if (condition != null)
//            {
//                var visitor = new ExamineVisitor();
//                visitor.Visit(condition);
//                query = visitor.Query;
//            }

//            var home = typeof(T) == typeof(Home) ? null : await GetHome();
//            var media = await Cache.FetchAsync<IEnumerable<Media>>(UmbracoConstant.UmbracoMedia);
//            var config = CmsRegistry.ConfigFor<T>();

//            if (config == null)
//            {
//                return null;
//            }

//            var contents = ContentFor(string.Format(ContentFormat, home?.CmsId.ToString() ?? string.Empty, config.Alias, query == null ? string.Empty : $" +({query})"));

//            return contents.Solve<T>(MediaDefer<T>(config, home, media)).ToList();
//        }

//        private static IEnumerable<IContent> ContentFor(string query)
//        {
//            if (string.IsNullOrWhiteSpace(query))
//            {
//                return null;
//            }

//            var searcher = ExamineManager.Instance.SearchProviderCollection[DataSearcher];
//            var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
//            searchCriteria.RawQuery(query);
//            var results = searcher.Search(searchCriteria);
//            return results.Select(x => new UmbracoContent(x.Fields)).ToList();
//        }

//        public string ToUrl(IEnumerable<Media> medias, string mediaKey)
//        {
//            var media = medias?.FirstOrDefault(x => x.Is(mediaKey));
//            return media?.UmbracoFile.Url;
//        }

//        private MapDefer MediaDefer<T>(MapConfig config, Home home, IEnumerable<Media> medias) where T : class, new()
//        {
//            if (config.MediaMaps == null || config.MediaMaps.Count <= 0)
//            {
//                return null;
//            }

//            var defer = new MapDefer();
//            var typeDefer = defer.For<T>();
//            var imageBase = home == null ? string.Empty : home.ImageBaseUrl;
//            foreach (var map in config.MediaMaps)
//            {
//                var info = map;
//                var alias = info.Name;
//                typeDefer.Attach<string>(alias, (x, id) =>
//                {
//                    if (string.IsNullOrWhiteSpace(id))
//                    {
//                        return;
//                    }

//                    var url = ToUrl(medias, id);

//                    if (url == null)
//                    {
//                        return;
//                    }

//                    info.SetValue(x, imageBase + url);
//                });
//            }

//            return defer;
//        }

//        public async Task<Home> GetHome()
//        {
//            var countryCode = Config.SiteSettings.CountryCode;
//            var all = await Cache.FetchTypeAsync<Home>();

//            return
//                all?.FirstOrDefault(
//                    x => string.Equals(x.CountryCode, countryCode, StringComparison.CurrentCultureIgnoreCase));
//        }

//        //private IEnumerable<UmbracoContent> ChildrenFor(IContent parent)
//        //{
//        //    var searcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
//        //    var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
//        //    searchCriteria.RawQuery("__path:" + string.Format("{0}*", parent.Get<string>("__path")));
//        //    var results = searcher.Search(searchCriteria);

//        //    return results.Select(x => new UmbracoContent(x.Fields)).ToList();
//        //}
//    }
//}
