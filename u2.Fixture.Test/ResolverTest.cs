using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using u2.Caching;
using u2.Core.Contract;
using u2.Fixture;
using u2.Test;

namespace u2.Config.Test
{
    [TestFixture]
    public class ResolverTest
    {
        [Test]
        public void Get_singleton_success()
        {
            Resolver.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());

            var first = Resolver.Get<IMapRegistry>();
            var second = Resolver.Get<IMapRegistry>();

            Assert.AreEqual(first, second);
        }

        [Test]
        public void Get_function_success()
        {
            var defaultCache = Substitute.For<ICache>();
            defaultCache.Fetch<TestRoot>().Returns(new List<TestRoot>
            {
                new TestRoot {Id = 1},
                new TestRoot {Id = 2}
            });
            SiteCaches.Default = defaultCache;
            Resolver.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());

            var first = Resolver.Get<IRoot>();
            var second = Resolver.Get<IRoot>();

            Assert.AreEqual(first, second);
            Assert.That(first.Id, Is.EqualTo(1));
        }
    }
}
