using System.Collections.Generic;
using System.Threading.Tasks;
using Ninject;
using u2.Caching;
using u2.Config.Contract;
using u2.Core.Contract;
using u2.Demo.Data;
using u2.Demo.Provider;

namespace u2.Demo.Api.Ninject
{
    public class CacheConfig : ICacheConfig
    {
        [Inject]
        public IDataProvider DataProvider { get; set; }

        public void Config(ICacheRegistry registry)
        {
            registry.Add(DataProvider.GetLabels);
        }
    }
}