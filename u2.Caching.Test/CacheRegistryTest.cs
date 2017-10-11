using System.Collections.Generic;
using System.Threading.Tasks;
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
    }
}
