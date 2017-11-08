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
        public ICacheRegistry CacheRegistry
        {
            get
            {
                var registry = new CacheRegistry();
                registry.Add(async () => await Task.Run(() => new[] {new CacheItem()} as IEnumerable<CacheItem>));
                return registry;
            }
        }

        [Test]
        public void Add_has_success()
        {
            Assert.That(CacheRegistry.Has<CacheItem>(), Is.True);
        }

        [Test]
        public void Add_has_key_success()
        {
            var key = typeof(CacheItem).FullName;
            Assert.That(CacheRegistry.Has(key), Is.True);
        }

        [Test]
        public void TryGetTask_success()
        {
            var taskKey = typeof(CacheItem).FullName;
            var result = CacheRegistry.TryGetTask(taskKey, out ICacheTask task);
            Assert.That(result, Is.True);
            Assert.That(task, Is.Not.Null);
        }

        [Test]
        public async Task ReloadAsync_no_key()
        {
            var task = Substitute.For<Func<Task<IEnumerable<TestItem>>>>();
            var cacheRegistry = new CacheRegistry();
            cacheRegistry.Add(task);
            await cacheRegistry.ReloadAsync();

            task.Received(1);
        }

        [Test]
        public void Reload_no_key()
        {
            var task = Substitute.For<Func<Task<IEnumerable<TestItem>>>>();
            var cacheRegistry = new CacheRegistry();
            cacheRegistry.Add(task);
            cacheRegistry.Reload();

            task.Received(1);
        }

        [Test]
        public async Task ReloadAsync_with_key()
        {
            var task = Substitute.For<Func<Task<IEnumerable<TestItem>>>>();
            var cacheRegistry = new CacheRegistry();
            cacheRegistry.Add(task, "test");
            await cacheRegistry.ReloadAsync();

            task.Received(1);
        }

        [Test]
        public void Reload_with_key()
        {
            var task = Substitute.For<Func<Task<IEnumerable<TestItem>>>>();
            var cacheRegistry = new CacheRegistry();
            cacheRegistry.Add(task, "test");
            cacheRegistry.Reload();

            task.Received(1);
        }
    }
}
