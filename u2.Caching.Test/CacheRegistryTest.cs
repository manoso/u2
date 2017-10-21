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
        private ICacheRegistry _cacheRegistry;

        [OneTimeSetUp]
        public void Setup()
        {
            _cacheRegistry = new CacheRegistry();
            _cacheRegistry.Add(async () => await Task.Run(() => new[] { new CacheItem() } as IEnumerable<CacheItem>));
        }

        [Test]
        public void Add_has_success()
        {
            Assert.That(_cacheRegistry.Has<CacheItem>(), Is.True);
        }

        [Test]
        public void Add_has_key_success()
        {
            var key = typeof(CacheItem).FullName;
            Assert.That(_cacheRegistry.Has(key), Is.True);
        }

        [Test]
        public void TryGetTask_success()
        {
            var taskKey = typeof(CacheItem).FullName;
            var result = _cacheRegistry.TryGetTask(taskKey, out ICacheTask task);
            Assert.That(result, Is.True);
            Assert.That(task, Is.Not.Null);
        }

        [Test]
        public void Reload_no_key()
        {
            var task = Substitute.For<Func<Task<IEnumerable<TestItem>>>>();
            _cacheRegistry = new CacheRegistry();
            _cacheRegistry.Add(task);
            _cacheRegistry.Reload();

            task.Received(1);
        }

        [Test]
        public void Reload_with_key()
        {
            var task = Substitute.For<Func<Task<IEnumerable<TestItem>>>>();
            _cacheRegistry = new CacheRegistry();
            _cacheRegistry.Add(task, "test");
            _cacheRegistry.Reload();

            task.Received(1);
        }
    }
}
