using u2.Core.Contract;

namespace u2.Umbraco
{
    using System.Collections.Generic;
    using System.Linq;
    using Examine;
    using Examine.SearchCriteria;

    public class UmbracoFetcher : ICmsFetcher
    {
        private const string U2Searcher = "U2Searcher";

        public IEnumerable<IContent> Fetch(ICmsQuery cmsQuery)
        {
            var query = cmsQuery.Query;

            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            var searcher = ExamineManager.Instance.SearchProviderCollection[U2Searcher];
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
