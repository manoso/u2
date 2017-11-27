using System;
using NSubstitute;
using NUnit.Framework;
using u2.Caching;
using u2.Caching.Contract;
using u2.Fixture.Contract;
using u2.Core;
using u2.Core.Contract;
using u2.Fixture;
using u2.Test;
using u2.Umbraco;
using u2.Umbraco.Contract;

namespace u2.Config.Test
{
    [TestFixture]
    public class BindConfigTest
    {
        
        [Test]
        public void Config_Success()
        {
            var binder = Substitute.For<IBinder>();

            var config = new BindConfig(binder);
            config.Config<TestRoot, TestUmbracoConfig, TestCacheConfig, TestMapBuild, TestCacheBuild>();

            binder.Received(1).Add<IMapRegistry, MapRegistry>(true);
            binder.Received(1).Add<IMapper, Mapper>(true);
            binder.Received(1).Add<ICacheRegistry, CacheRegistry>(true);
            binder.Received(1).Add<IQueryFactory, UmbracoQueryFactory>(true);
            binder.Received(1).Add<ICmsFetcher, UmbracoFetcher>(true);
            binder.Received(1).Add<IRegistry, Registry>(true);
            binder.Received(1).Add<ICacheStore, CacheStore>();
            binder.Received(1).Add<IUmbracoConfig, TestUmbracoConfig>(true);
            binder.Received(1).Add<ICacheConfig, TestCacheConfig>(true);
            binder.Received(1).Add<IMapBuild, TestMapBuild>(true);
            binder.Received(1).Add<ICacheBuild, TestCacheBuild>(true);
            binder.Received(1).Add<IRoot, TestRoot>(false, Arg.Any<Func<TestRoot>>());
            binder.Received(1).Add<ICache, Cache>(false, Arg.Any<Func<Cache>>());

            binder.Received(1).Get<IRegistry>();
            binder.Received(1).Get<IMapBuild>();
            binder.Received(1).Get<ICacheRegistry>();
            binder.Received(1).Get<ICacheBuild>();
        }
    }
}
