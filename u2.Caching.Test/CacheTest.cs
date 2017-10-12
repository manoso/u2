using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;

namespace u2.Caching.Test
{
    [TestFixture]
    public class CacheTest
    {
        [Test]
        public void FetchAsync_without_key_success()
        {
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(cacheStore, cacheRegistry);

            cache.FetchAsync<TestItem>();

            var key = typeof(TestItem).FullName;
            cacheRegistry.Received(1).TryGetTask(key, out ICacheTask _);
        }

        [Test]
        public void FetchAsync_with_key_success()
        {
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(cacheStore, cacheRegistry);

            var key = "key";
            cache.FetchAsync<TestItem>();

            cacheRegistry.Received(1).TryGetTask(typeof(TestItem).FullName, out ICacheTask _);
        }
    }
}
