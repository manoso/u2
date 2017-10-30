using NUnit.Framework;
using u2.Test;

namespace u2.Caching.Test
{
    [TestFixture]
    public class CacheStoreTest
    {
        [Test]
        public void Save_Get()
        {
            var store = new CacheStore();
            var item = new TestItem();
            store.Save("test", item);
            var result = store.Get("test");
            Assert.That(result, Is.EqualTo(result));
        }

        [Test]
        public void Save_Has()
        {
            var store = new CacheStore();
            var item = new TestItem();
            store.Save("test", item);
            var result = store.Has("test");
            Assert.That(result);
        }

        [Test]
        public void Clear()
        {
            var store = new CacheStore();
            var item = new TestItem();
            store.Save("test", item);
            store.Clear("test");
            var result = store.Has("test");
            Assert.That(!result);
        }
    }
}
