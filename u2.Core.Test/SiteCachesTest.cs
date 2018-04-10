using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class SiteCachesTest
    {
        [Test]
        public void Default_not_null()
        {
            var cacheRegistry = Substitute.For<ICacheRegistry>();
            var siteCaches = new SiteCaches(cacheRegistry);

            Assert.NotNull(siteCaches.Default);
        }

        [Test]
        public async Task RefreshAsync_with_null_success()
        {
            var cacheRegistry = Substitute.For<ICacheRegistry>();
            var siteCaches = new SiteCaches(cacheRegistry);
            var rootAu = new TestSite
            {
                Key = Guid.NewGuid(),
                SiteName = "AU"
            };
            var rootNz = new TestSite
            {
                Key = Guid.NewGuid(),
                SiteName = "NZ"
            };
            var cacheAu = siteCaches.Get(rootAu);
            var cacheNz = siteCaches.Get(rootNz);

            await siteCaches.RefreshAsync().ConfigureAwait(false);

            await cacheRegistry.Received(1).ReloadAsync(cacheAu).ConfigureAwait(false);
            await cacheRegistry.Received(1).ReloadAsync(cacheNz).ConfigureAwait(false);
        }

        [Test]
        public void Refresh_with_null_success()
        {
            var cacheRegistry = Substitute.For<ICacheRegistry>();
            var siteCaches = new SiteCaches(cacheRegistry);
            var rootAu = new TestSite
            {
                Key = Guid.NewGuid(),
                SiteName = "AU"
            };
            var rootNz = new TestSite
            {
                Key = Guid.NewGuid(),
                SiteName = "NZ"
            };
            var cacheAu = siteCaches.Get(rootAu);
            var cacheNz = siteCaches.Get(rootNz);

            siteCaches.Refresh();

            cacheRegistry.Received(1).ReloadAsync(cacheAu);
            cacheRegistry.Received(1).ReloadAsync(cacheNz);
        }

        [Test]
        public async Task RefreshAsync_not_null_success()
        {
            var cacheRegistry = Substitute.For<ICacheRegistry>();
            var siteCaches = new SiteCaches(cacheRegistry);
            var rootAu = new TestSite
            {
                Key = Guid.NewGuid(),
                SiteName = "AU"
            };
            var rootNz = new TestSite
            {
                Key = Guid.NewGuid(),
                SiteName = "NZ"
            };
            var cacheAu = siteCaches.Get(rootAu);
            var cacheNz = siteCaches.Get(rootNz);

            await siteCaches.RefreshAsync(rootAu).ConfigureAwait(false);

            await cacheRegistry.Received(1).ReloadAsync(cacheAu).ConfigureAwait(false);
            await cacheRegistry.DidNotReceive().ReloadAsync(cacheNz).ConfigureAwait(false);

        }

        [Test]
        public void Refresh_not_null_success()
        {
            var cacheRegistry = Substitute.For<ICacheRegistry>();
            var siteCaches = new SiteCaches(cacheRegistry);
            var rootAu = new TestSite
            {
                Key = Guid.NewGuid(),
                SiteName = "AU"
            };
            var rootNz = new TestSite
            {
                Key = Guid.NewGuid(),
                SiteName = "NZ"
            };
            var cacheAu = siteCaches.Get(rootAu);
            var cacheNz = siteCaches.Get(rootNz);

            siteCaches.Refresh(rootAu);

            cacheRegistry.Received(1).ReloadAsync(cacheAu);
            cacheRegistry.DidNotReceive().ReloadAsync(cacheNz);
        }
    }
}
