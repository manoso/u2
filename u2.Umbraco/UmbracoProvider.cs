//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;
//using Cinema.Data.Cache;
//using Cinema.Data.Cms.DataType;
//using Cinema.Data.Cms.Helper;
//using Cinema.Data.Cms.Mapping;
//using Cinema.Data.Model.Cms.Section;
//using Cinema.Infrastructure.Configuration;
//using Cinema.Infrastructure.Ninject;
//using Examine;
//using Examine.SearchCriteria;
//using Ninject;
//using Cinema.Infrastructure.Log;

//namespace Cinema.Data.Umbraco
//{
//    public interface IUmbracoProvider : IRequestScope
//    {
//        Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> condition = null) where T : class, new();
//        Task<IEnumerable<KeyValuePair<string, Media>>> GetMedia();
//        Task<Home> GetHome();
//    }

//    public class UmbracoProvider : IUmbracoProvider
//    {
//        private const string DataSearcher = "DataSearcher";

//        private const string ContentFormat = @"+__IndexType:content +__Path:\-1,{0}* +__NodeTypeAlias:{1}{2}";
//        private const string MediaFormat = "+__IndexType:media";

//        private const string MediaIdAlias = "id";
//        private const string MediaUrlAlias = "umbracoFile";

//        [Inject]
//        public ICinemaConfiguration Config { get; set; }

//        [Inject]
//        public IUmbracoCache UmbracoCache { get; set; }

//        [Inject]
//        public ICinemaLog CinemaLog { get; set; }

//        public async Task<IEnumerable<KeyValuePair<string, Media>>> GetMedia()
//        {
//            var result = await ContentFor(MediaFormat);
//            return result?.ToDictionary(x => x.Get<string>(MediaIdAlias), x => x.Get<string>(MediaUrlAlias)?.JsonTo<Media>());
//        }

//        public async Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> condition = null) where T : class, new()
//        {
//            string query = null;
//            if (condition != null)
//            {
//                var visitor = new ExamineVisitor();
//                visitor.Visit(condition);
//                query = visitor.Query;
//            }

//            var home = typeof(T) == typeof(Home) ? null : await GetHome();
//            var medias = await UmbracoCache.GetMedias();
//            var config = CmsRegistry.ConfigFor<T>();
//            if (config == null)
//                return null;

//            var contents = await ContentFor(string.Format(ContentFormat, home?.CmsId.ToString() ?? string.Empty, config.Alias, query == null ? string.Empty : $" +({query})"));
//            return contents.Solve<T>(MediaDefer<T>(config, home, medias)).ToList();
//        }

//        private static async Task<IEnumerable<IContent>> ContentFor(string query)
//        {
//            return await Task.Run(() =>
//            {
//                if (string.IsNullOrWhiteSpace(query))
//                    return null;

//                var searcher = ExamineManager.Instance.SearchProviderCollection[DataSearcher];
//                var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
//                searchCriteria.RawQuery(query);
//                var results = searcher.Search(searchCriteria);
//                return results.Select(x => new UmbracoContent(x.Fields)).ToList() as IEnumerable<IContent>;
//            });
//        }

//        public string ToUrl(IDictionary<string, Media> medias, string mediaId)
//        {
//            Media media = null;
//            medias?.TryGetValue(mediaId, out media);
//            return media?.Url;
//        }

//        private MapDefer MediaDefer<T>(MapConfig config, Home home, IDictionary<string, Media> medias) where T: class, new()
//        {
//            if (config.MediaMaps == null || config.MediaMaps.Count <= 0)
//                return null;

//            var defer = new MapDefer();
//            var typeDefer = defer.For<T>();
//            var imageBase = home == null ? string.Empty : home.ImageBaseUrl;
//            foreach (var map in config.MediaMaps)
//            {
//                var info = map;
//                var alias = info.Name;
//                typeDefer.Attach<string>(alias, (x, id) =>
//                {
//                    if (string.IsNullOrWhiteSpace(id)) return;

//                    var url = ToUrl(medias, id);
//                    if (url == null) return;

//                    info.SetValue(x, imageBase + url);
//                });
//            }
//            return defer;
//        }

//        public async Task<Home> GetHome()
//        {
//            var countryCode = Config.SiteSettings.CountryCode;
//            var all = await UmbracoCache.GetHomes();

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
