using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;

namespace u2.Caching.Test
{
    [TestFixture]
    public class CacheRegistryTest
    {
        public ICacheRegistry CacheRegistryAddHasCache
        {
            get
            {
                var registry = new CacheRegistry(new TestCacheConfig());
                registry.Add(async x => await Task.Run(() => new[] { new CacheItem() } as IEnumerable<CacheItem>).ConfigureAwait(false));
                return registry;
            }
        }

        public ICacheRegistry CacheRegistryAddNoCache
        {
            get
            {
                var registry = new CacheRegistry(new TestCacheConfig());
                registry.Add(async () => await Task.Run(() => new[] { new CacheItem() } as IEnumerable<CacheItem>).ConfigureAwait(false));
                return registry;
            }
        }

        [Test]
        public void AddHasCache_has_success()
        {
            Assert.That(CacheRegistryAddHasCache.Has<CacheItem>(), Is.True);
        }

        [Test]
        public void AddHasCache_has_key_success()
        {
            var key = typeof(CacheItem).FullName;
            Assert.That(CacheRegistryAddHasCache.Has(key), Is.True);
        }

        [Test]
        public void AddHasCache_TryGetTask_success()
        {
            var taskKey = typeof(CacheItem).FullName;
            var result = CacheRegistryAddHasCache.TryGetTask(taskKey, out ICacheTask task);
            Assert.That(result, Is.True);
            Assert.That(task, Is.Not.Null);
        }

        [Test]
        public void AddNoCache_has_success()
        {
            Assert.That(CacheRegistryAddNoCache.Has<CacheItem>(), Is.True);
        }

        [Test]
        public void AddNoCache_has_key_success()
        {
            var key = typeof(CacheItem).FullName;
            Assert.That(CacheRegistryAddNoCache.Has(key), Is.True);
        }

        [Test]
        public void AddNoCache_TryGetTask_success()
        {
            var taskKey = typeof(CacheItem).FullName;
            var result = CacheRegistryAddNoCache.TryGetTask(taskKey, out ICacheTask task);
            Assert.That(result, Is.True);
            Assert.That(task, Is.Not.Null);
        }

        [Test]
        public async Task ReloadAsync_no_key()
        {
            var task = Substitute.For<Func<ICache, Task<IEnumerable<TestItem>>>>();
            var cacheRegistry = new CacheRegistry(new TestCacheConfig());
            cacheRegistry.Add(task);
            await cacheRegistry.ReloadAsync(Arg.Any<ICache>()).ConfigureAwait(false);

            task.Received(1);
        }

        [Test]
        public void Reload_no_key()
        {
            var task = Substitute.For<Func<ICache, Task<IEnumerable<TestItem>>>>();
            var cacheRegistry = new CacheRegistry(new TestCacheConfig());
            cacheRegistry.Add(task);
            cacheRegistry.Reload(Arg.Any<ICache>());

            task.Received(1);
        }

        [Test]
        public async Task ReloadAsync_with_key()
        {
            var task = Substitute.For<Func<ICache, Task<IEnumerable<TestItem>>>>();
            var cacheRegistry = new CacheRegistry(new TestCacheConfig());
            cacheRegistry.Add(task, "test");
            await cacheRegistry.ReloadAsync(Arg.Any<ICache>()).ConfigureAwait(false);

            task.Received(1);
        }

        [Test]
        public void Reload_with_key()
        {
            var task = Substitute.For<Func<ICache, Task<IEnumerable<TestItem>>>>();
            var cacheRegistry = new CacheRegistry(new TestCacheConfig());
            cacheRegistry.Add(task, "test");
            cacheRegistry.Reload(Arg.Any<ICache>());

            task.Received(1);
        }
    }
}
