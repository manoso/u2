using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;

namespace u2.Caching.Test
{
    [TestFixture]
    public class SiteCachesTest
    {
        [Test]
        public async Task RefreshAsync_with_null_success()
        {
            var cacheAu = Substitute.For<ICache>();
            var cacheNz = Substitute.For<ICache>();

            var siteCaches = new SiteCaches(null)
            {
                [new TestRoot { Key = Guid.NewGuid() }] = cacheAu,
                [new TestRoot { Key = Guid.NewGuid() }] = cacheNz
            };

            await siteCaches.RefreshAsync();

            await cacheAu.Received(1).ReloadAsync();
            await cacheNz.Received(1).ReloadAsync();
        }

        [Test]
        public void Refresh_with_null_success()
        {
            var cacheAu = Substitute.For<ICache>();
            var cacheNz = Substitute.For<ICache>();

            var siteCaches = new SiteCaches(null)
            {
                [new TestRoot { Key = Guid.NewGuid() }] = cacheAu,
                [new TestRoot { Key = Guid.NewGuid() }] = cacheNz
            };

            siteCaches.Refresh();

            cacheAu.Received(1).ReloadAsync();
            cacheNz.Received(1).ReloadAsync();
        }

        [Test]
        public async Task RefreshAsync_not_null_success()
        {
            var cacheAu = Substitute.For<ICache>();
            var cacheNz = Substitute.For<ICache>();
            var au = new TestRoot { Key = Guid.NewGuid() };
            var nz = new TestRoot { Key = Guid.NewGuid() };

            var siteCaches = new SiteCaches(null)
            {
                [au] = cacheAu,
                [nz] = cacheNz
            };

            await siteCaches.RefreshAsync(au);

            await cacheAu.Received(1).ReloadAsync();
            await cacheNz.DidNotReceive().ReloadAsync();
        }

        [Test]
        public void Refresh_not_null_success()
        {
            var cacheAu = Substitute.For<ICache>();
            var cacheNz = Substitute.For<ICache>();
            var au = new TestRoot { Key = Guid.NewGuid() };
            var nz = new TestRoot { Key = Guid.NewGuid() };

            var siteCaches = new SiteCaches(null)
            {
                [au] = cacheAu,
                [nz] = cacheNz
            };

            siteCaches.Refresh(au);

            cacheAu.Received(1).ReloadAsync();
            cacheNz.DidNotReceive().ReloadAsync();
        }
    }
}
