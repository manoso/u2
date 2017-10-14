using System.Collections.Generic;
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
        public void Register_called_CacheRegistry_Add()
        {
            var registryAu = Substitute.For<ICacheRegistry>();

            var cache = new SiteCaches
            {
                ["au"] = registryAu,
            };

            async Task<IEnumerable<CacheItem>> Func() => await Task.Run(() => new[] {new CacheItem()} as IEnumerable<CacheItem>);

            cache.Register(Func);

            registryAu.Received(1).Add(Func, typeof(CacheItem).FullName).Span(SiteCaches.DefaultCache);
        }

        [Test]
        public void Register_2_called_CacheRegistry_Add()
        {
            var registryAu = Substitute.For<ICacheRegistry>();
            var registryNz = Substitute.For<ICacheRegistry>();

            var cache = new SiteCaches
            {
                ["au"] = registryAu,
                ["nz"] = registryNz
            };

            async Task<IEnumerable<CacheItem>> Func() => await Task.Run(() => new[] { new CacheItem() } as IEnumerable<CacheItem>);

            var cacheKey = "key";
            cache.Register(cacheKey, Func);

            registryAu.Received(1).Add(Func, cacheKey).Span(SiteCaches.DefaultCache);
            registryNz.Received(1).Add(Func, cacheKey).Span(SiteCaches.DefaultCache);
        }

        [Test]
        public void RegisterLookup_called_CacheRegistry_Add()
        {
            var registryAu = Substitute.For<ICacheRegistry>();
            var lookup = Substitute.For<ICacheLookup<CacheItem>>();

            var cache = new SiteCaches
            {
                ["au"] = registryAu,
            };

            async Task<IEnumerable<CacheItem>> Func() => await Task.Run(() => new[] { new CacheItem() } as IEnumerable<CacheItem>);

            cache.RegisterLookup(Func, 0, lookup);

            registryAu.Received(1).Add(Func).Lookup(lookup).Span(SiteCaches.DefaultCache);
        }

        [Test]
        public async Task Refresh_with_null_success()
        {
            var registryAu = Substitute.For<ICacheRegistry>();
            var registryNz = Substitute.For<ICacheRegistry>();

            var cache = new SiteCaches
            {
                ["au"] = registryAu,
                ["nz"] = registryNz
            };

            await cache.Refresh();

            await registryAu.Received(1).Reload();
            await registryNz.Received(1).Reload();
        }

        [Test]
        public async Task Refresh_not_null_success()
        {
            var registryAu = Substitute.For<ICacheRegistry>();
            var registryNz = Substitute.For<ICacheRegistry>();
            var au = "au";
            var nz = "nz";

            var cache = new SiteCaches
            {
                [au] = registryAu,
                [nz] = registryNz
            };

            await cache.Refresh(au);

            await registryAu.Received(1).Reload();
            await registryNz.DidNotReceive().Reload();
        }
    }
}
