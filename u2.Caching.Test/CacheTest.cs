using System;
using System.Threading.Tasks;
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
        public async Task FetchAsync_without_key_success()
        {
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(cacheStore, cacheRegistry);

            await cache.FetchAsync<TestItem>();

            var key = typeof(TestItem).FullName;
            cacheRegistry.Received(1).TryGetTask(key, out ICacheTask _);
        }

        [Test]
        public async Task FetchAsync_with_key_success()
        {
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(cacheStore, cacheRegistry);

            var key = "key";
            await cache.FetchAsync<TestItem>(key);

            cacheRegistry.Received(1).TryGetTask(key, out ICacheTask _);
        }

        [Test]
        public async Task FetchAsync_TaskRun_called()
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

            await cache.FetchAsync<TestItem>();

            await task.Received(1).Run(Arg.Any<Action<string, object>>());
        }

        [Test]
        public async Task FetchAsync_no_task_returns_null()
        {
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            cacheRegistry.TryGetTask(Arg.Any<string>(), out ICacheTask _)
                .Returns(false);

            var cache = new Cache(null, cacheRegistry);

            var result = await cache.FetchAsync<TestItem>();

            Assert.That(result, Is.Null);
        }
    }
}
