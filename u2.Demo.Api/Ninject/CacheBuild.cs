using Ninject;
using u2.Fixture.Contract;
using u2.Core.Contract;
using u2.Demo.Provider;

namespace u2.Demo.Api.Ninject
{
    public class CacheBuild : ICacheBuild
    {
        [Inject]
        public IDataProvider DataProvider { get; set; }

        public void Setup(ICacheRegistry registry)
        {
            registry.Add(DataProvider.GetLabels);
        }
    }
}