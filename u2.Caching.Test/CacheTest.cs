using System;
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
            cache.FetchAsync<TestItem>(key);

            cacheRegistry.Received(1).TryGetTask(key, out ICacheTask _);
        }

        [Test]
        public void FetchAsync_TaskRun_called()
        {
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();
            var task = Substitute.For<ICacheTask>();

            cacheRegistry.TryGetTask(Arg.Any<string>(), out ICacheTask _)
                .Returns(x => {
                    x[1] = task;
                    return true;
                });
            var cache = new Cache(cacheStore, cacheRegistry);
            cacheStore.Has(Arg.Any<string>()).Returns(false);

            cache.FetchAsync<TestItem>();

            task.Received(1).Run(Arg.Any<Action<string, object>>());
        }
    }
}
