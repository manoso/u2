using u2.Core.Contract;
using u2.Demo.Provider;
using u2.Fixture.Contract;

namespace u2.Demo.Api.Ninject
{
    public class ResolverCacheBuild : ICacheBuild
    {
        public void Setup(ICacheRegistry registry)
        {
            registry.Add(new DataProvider().GetLabels);
        }
    }
}