using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using u2.Core.Contract;
using u2.Umbraco;

namespace u2.Test
{
    public class TestCmsFetcher : ICmsFetcher
    {
        private readonly IDictionary<ICmsQuery, IList<IDictionary<string, string>>> _contents = new Dictionary<ICmsQuery, IList<IDictionary<string, string>>>();

        public void Add(ICmsQuery key, IList<IDictionary<string, string>> value)
        {
            _contents.Add(key, value);
        }

        public IEnumerable<IContent> Fetch(ICmsQuery cmsQuery)
        {
            var items = _contents[cmsQuery];
            return items.Select(x => new UmbracoContent(x)).ToList();
        }
    }
}
