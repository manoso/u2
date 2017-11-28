using u2.Core.Contract;
using u2.Umbraco.Contract;

namespace u2.Umbraco
{
    using System.Collections.Generic;
    using System.Linq;
    using Examine;
    using Examine.SearchCriteria;

    public class UmbracoFetcher : ICmsFetcher
    {
        private readonly IUmbracoConfig _config;
        public UmbracoFetcher(IUmbracoConfig config)
        {
            _config = config;
        }

        public IEnumerable<IContent> Fetch(ICmsQuery cmsQuery)
        {
            var query = cmsQuery.Query;

            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            var searcher = ExamineManager.Instance.SearchProviderCollection[_config.ExamineSearcher];
            var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.Or);
            searchCriteria.RawQuery(query);
            var results = searcher.Search(searchCriteria);
            return results.Select(x => new UmbracoContent(x.Fields)).ToList();
        }
    }
}
