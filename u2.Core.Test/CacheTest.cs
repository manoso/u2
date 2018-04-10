using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class CacheTest
    {
        [Test]
        public async Task FetchAsync_without_key_success()
        {
            var root = Substitute.For<ISite>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(cacheStore, cacheRegistry, root);

            await cache.FetchAsync<TestItem>().ConfigureAwait(false);

            var key = typeof(TestItem).FullName;
            cacheRegistry.Received(1).TryGetTask(key, out ICacheTask _);
        }

        [Test]
        public async Task FetchAsync_with_key_success()
        {
            var root = Substitute.For<ISite>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(cacheStore, cacheRegistry, root);

            var key = "key";
            await cache.FetchAsync<TestItem>(key).ConfigureAwait(false);

            cacheRegistry.Received(1).TryGetTask(key, out ICacheTask _);
        }

        [Test]
        public async Task FetchAsync_TaskRun_called()
        {
            var root = Substitute.For<ISite>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();
            var task = Substitute.For<ICacheTask>();

            cacheRegistry.TryGetTask(Arg.Any<string>(), out ICacheTask _)
                .Returns(x => {
                    x[1] = task;
                    return true;
                });
            var cache = new Cache(cacheStore, cacheRegistry, root);
            cacheStore.Has(Arg.Any<string>()).Returns(false);

            await cache.FetchAsync<TestItem>().ConfigureAwait(false);

            await task.Received(1).Run(cache, Arg.Any<Action<string, object>>()).ConfigureAwait(false);
        }

        [Test]
        public async Task FetchAsync_no_task_returns_null()
        {
            var root = Substitute.For<ISite>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            cacheRegistry.TryGetTask(Arg.Any<string>(), out ICacheTask _)
                .Returns(false);

            var cache = new Cache(null, cacheRegistry, root);

            var result = await cache.FetchAsync<TestItem>().ConfigureAwait(false);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Fetch_without_key_success()
        {
            var root = Substitute.For<ISite>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(cacheStore, cacheRegistry, root);

            cache.Fetch<TestItem>();

            var key = typeof(TestItem).FullName;
            cacheRegistry.Received(1).TryGetTask(key, out ICacheTask _);
        }

        [Test]
        public void Fetch_with_key_success()
        {
            var root = Substitute.For<ISite>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(cacheStore, cacheRegistry, root);

            var key = "key";
            cache.Fetch<TestItem>(key);

            cacheRegistry.Received(1).TryGetTask(key, out ICacheTask _);
        }

        [Test]
        public void Fetch_TaskRun_called()
        {
            var root = Substitute.For<ISite>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();
            var task = Substitute.For<ICacheTask>();

            cacheRegistry.TryGetTask(Arg.Any<string>(), out ICacheTask _)
                .Returns(x => {
                    x[1] = task;
                    return true;
                });
            var cache = new Cache(cacheStore, cacheRegistry, root);
            cacheStore.Has(Arg.Any<string>()).Returns(false);

            cache.Fetch<TestItem>();

            task.Received(1).Run(cache, Arg.Any<Action<string, object>>());
        }

        [Test]
        public void Fetch_no_task_returns_null()
        {
            var root = Substitute.For<ISite>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            cacheRegistry.TryGetTask(Arg.Any<string>(), out ICacheTask _)
                .Returns(false);

            var cache = new Cache(null, cacheRegistry, root);

            var result = cache.Fetch<TestItem>();

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task ReloadAsync_type_with_key_success()
        {
            var root = Substitute.For<ISite>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(cacheStore, cacheRegistry, root);

            await cache.ReloadAsync<TestItem>("key").ConfigureAwait(false);

            await cacheRegistry.Received(1).ReloadAsync<TestItem>(cache, "key").ConfigureAwait(false);
        }

        [Test]
        public async Task ReloadAsync_type_no_key_success()
        {
            var root = Substitute.For<ISite>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(cacheStore, cacheRegistry, root);

            await cache.ReloadAsync<TestItem>().ConfigureAwait(false);

            await cacheRegistry.Received(1).ReloadAsync<TestItem>(cache, Arg.Any<string>()).ConfigureAwait(false);
        }

        [Test]
        public async Task Reload_type_with_key_success()
        {
            var root = Substitute.For<ISite>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(cacheStore, cacheRegistry, root);

            cache.Reload<TestItem>("key");

            await cacheRegistry.Received(1).ReloadAsync<TestItem>(cache, "key").ConfigureAwait(false);
        }

        [Test]
        public async Task Reload_type_no_key_success()
        {
            var root = Substitute.For<ISite>();
            var cacheStore = Substitute.For<ICacheStore>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();

            var cache = new Cache(cacheStore, cacheRegistry, root);

            cache.Reload<TestItem>();

            await cacheRegistry.Received(1).ReloadAsync<TestItem>(cache, Arg.Any<string>()).ConfigureAwait(false);
        }
    }
}
