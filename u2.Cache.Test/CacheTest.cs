using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace u2.Cache.Test
{
    [TestFixture]
    public class CacheTest
    {
        [Test]
        public void Register_called_CacheRegistry_Add()
        {
            var registryAu = Substitute.For<ICacheRegistry>();

            var cache = new Cache
            {
                ["au"] = registryAu,
            };

            async Task<IEnumerable<CacheItem>> Func() => await Task.Run(() => new[] {new CacheItem()} as IEnumerable<CacheItem>);

            cache.Register(Func);

            registryAu.Received(1).Add(typeof(CacheItem).FullName, Func, Cache.DefaultCache);
        }

        [Test]
        public void Register_2_called_CacheRegistry_Add()
        {
            var registryAu = Substitute.For<ICacheRegistry>();
            var registryNz = Substitute.For<ICacheRegistry>();

            var cache = new Cache
            {
                ["au"] = registryAu,
                ["nz"] = registryNz
            };

            async Task<IEnumerable<CacheItem>> Func() => await Task.Run(() => new[] { new CacheItem() } as IEnumerable<CacheItem>);

            var cacheKey = "key";
            cache.Register(cacheKey, Func);

            registryAu.Received(1).Add(cacheKey, Func, Cache.DefaultCache);
            registryNz.Received(1).Add(cacheKey, Func, Cache.DefaultCache);
        }

        [Test]
        public void RegisterLookup_called_CacheRegistry_Add()
        {
            var registryAu = Substitute.For<ICacheRegistry>();
            var lookup = Substitute.For<LookupParameter<CacheItem>>();

            var cache = new Cache
            {
                ["au"] = registryAu,
            };

            async Task<IEnumerable<CacheItem>> Func() => await Task.Run(() => new[] { new CacheItem() } as IEnumerable<CacheItem>);

            cache.RegisterLookup(Func, 0, lookup);

            registryAu.Received(1).Add(Func, Cache.DefaultCache, lookup);
        }

        [Test]
        public async Task Refresh_called_null()
        {
            var registryAu = Substitute.For<ICacheRegistry>();
            var registryNz = Substitute.For<ICacheRegistry>();

            var cache = new Cache
            {
                ["au"] = registryAu,
                ["nz"] = registryNz
            };

            await cache.Refresh();

            await registryAu.Received(1).Reload();
            await registryNz.Received(1).Reload();
        }

        [Test]
        public async Task Refresh_called_not_null()
        {
            var registryAu = Substitute.For<ICacheRegistry>();
            var registryNz = Substitute.For<ICacheRegistry>();
            var au = "au";
            var nz = "nz";

            var cache = new Cache
            {
                [au] = registryAu,
                [nz] = registryNz
            };

            await cache.Refresh(au);

            await registryAu.Received(1).Reload();
            await registryNz.DidNotReceive().Reload();
        }
    }

    public class CacheItem
    {
        public int LookupKey { get; set; }
    }
}
