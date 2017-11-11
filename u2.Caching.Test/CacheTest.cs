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
            var root = Substitute.For<IRoot>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(root, cacheStore, cacheRegistry);

            await cache.FetchAsync<TestItem>();

            var key = typeof(TestItem).FullName;
            cacheRegistry.Received(1).TryGetTask(key, out ICacheTask _);
        }

        [Test]
        public async Task FetchAsync_with_key_success()
        {
            var root = Substitute.For<IRoot>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(root, cacheStore, cacheRegistry);

            var key = "key";
            await cache.FetchAsync<TestItem>(key);

            cacheRegistry.Received(1).TryGetTask(key, out ICacheTask _);
        }

        [Test]
        public async Task FetchAsync_TaskRun_called()
        {
            var root = Substitute.For<IRoot>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();
            var task = Substitute.For<ICacheTask>();

            cacheRegistry.TryGetTask(Arg.Any<string>(), out ICacheTask _)
                .Returns(x => {
                    x[1] = task;
                    return true;
                });
            var cache = new Cache(root, cacheStore, cacheRegistry);
            cacheStore.Has(Arg.Any<string>()).Returns(false);

            await cache.FetchAsync<TestItem>();

            await task.Received(1).Run(cache, Arg.Any<Action<string, object>>());
        }

        [Test]
        public async Task FetchAsync_no_task_returns_null()
        {
            var root = Substitute.For<IRoot>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            cacheRegistry.TryGetTask(Arg.Any<string>(), out ICacheTask _)
                .Returns(false);

            var cache = new Cache(root, null, cacheRegistry);

            var result = await cache.FetchAsync<TestItem>();

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Fetch_without_key_success()
        {
            var root = Substitute.For<IRoot>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(root, cacheStore, cacheRegistry);

            cache.Fetch<TestItem>();

            var key = typeof(TestItem).FullName;
            cacheRegistry.Received(1).TryGetTask(key, out ICacheTask _);
        }

        [Test]
        public void Fetch_with_key_success()
        {
            var root = Substitute.For<IRoot>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(root, cacheStore, cacheRegistry);

            var key = "key";
            cache.Fetch<TestItem>(key);

            cacheRegistry.Received(1).TryGetTask(key, out ICacheTask _);
        }

        [Test]
        public void Fetch_TaskRun_called()
        {
            var root = Substitute.For<IRoot>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();
            var task = Substitute.For<ICacheTask>();

            cacheRegistry.TryGetTask(Arg.Any<string>(), out ICacheTask _)
                .Returns(x => {
                    x[1] = task;
                    return true;
                });
            var cache = new Cache(root, cacheStore, cacheRegistry);
            cacheStore.Has(Arg.Any<string>()).Returns(false);

            cache.Fetch<TestItem>();

            task.Received(1).Run(cache, Arg.Any<Action<string, object>>());
        }

        [Test]
        public void Fetch_no_task_returns_null()
        {
            var root = Substitute.For<IRoot>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            cacheRegistry.TryGetTask(Arg.Any<string>(), out ICacheTask _)
                .Returns(false);

            var cache = new Cache(root, null, cacheRegistry);

            var result = cache.Fetch<TestItem>();

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task ReloadAsync_type_with_key_success()
        {
            var root = Substitute.For<IRoot>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(root, cacheStore, cacheRegistry);

            await cache.ReloadAsync<TestItem>("key");

            await cacheRegistry.Received(1).ReloadAsync<TestItem>(cache, "key");
        }

        [Test]
        public async Task ReloadAsync_type_no_key_success()
        {
            var root = Substitute.For<IRoot>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(root, cacheStore, cacheRegistry);

            await cache.ReloadAsync<TestItem>();

            await cacheRegistry.Received(1).ReloadAsync<TestItem>(cache, Arg.Any<string>());
        }

        [Test]
        public async Task Reload_type_with_key_success()
        {
            var root = Substitute.For<IRoot>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(root, cacheStore, cacheRegistry);

            cache.Reload<TestItem>("key");

            await cacheRegistry.Received(1).ReloadAsync<TestItem>(cache, "key");
        }

        [Test]
        public async Task Reload_type_no_key_success()
        {
            var root = Substitute.For<IRoot>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(root, cacheStore, cacheRegistry);

            cache.Reload<TestItem>();

            await cacheRegistry.Received(1).ReloadAsync<TestItem>(cache, Arg.Any<string>());
        }
    }
}
