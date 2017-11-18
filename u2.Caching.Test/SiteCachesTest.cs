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

            SiteCaches.Add(new TestRoot { Key = Guid.NewGuid() }, cacheAu);
            SiteCaches.Add(new TestRoot { Key = Guid.NewGuid() }, cacheNz);

            await SiteCaches.RefreshAsync();

            await cacheAu.Received(1).ReloadAsync();
            await cacheNz.Received(1).ReloadAsync();
        }

        [Test]
        public void Refresh_with_null_success()
        {
            var cacheAu = Substitute.For<ICache>();
            var cacheNz = Substitute.For<ICache>();

            SiteCaches.Add(new TestRoot { Key = Guid.NewGuid() }, cacheAu);
            SiteCaches.Add(new TestRoot { Key = Guid.NewGuid() }, cacheNz);

            SiteCaches.Refresh();

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

            SiteCaches.Add(au, cacheAu);
            SiteCaches.Add(nz, cacheNz);


            await SiteCaches.RefreshAsync(au);

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

            SiteCaches.Add(au, cacheAu);
            SiteCaches.Add(nz, cacheNz);

            SiteCaches.Refresh(au);

            cacheAu.Received(1).ReloadAsync();
            cacheNz.DidNotReceive().ReloadAsync();
        }
    }
}
