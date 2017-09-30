using u2.Core.Contract;

namespace u2.Umbraco
{
    using System.Collections.Generic;
    using System.Linq;
    using Examine;
    using Examine.SearchCriteria;

    public class UmbracoFetcher : ICmsFetcher
    {
        private const string DataSearcher = "DataSearcher";

        public IEnumerable<IContent> Fetch<T>(ICmsQuery<T> cmsQuery)
            where T : class, new()
        {
            var query = cmsQuery.Query;

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

        //private IEnumerable<IContent> ChildrenFor(IContent parent)
        //{
        //    var searcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
        //    var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
        //    searchCriteria.RawQuery("__path:" + $"{parent.Get<string>("__path")}*");
        //    var results = searcher.Search(searchCriteria);

        //    return results.Select(x => new UmbracoContent(x.Fields)).ToList();
        //}
    }
}
