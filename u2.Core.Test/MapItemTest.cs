using System.Threading.Tasks;
using NUnit.Framework;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class MapItemTest
    {
        [Test]
        public void Constructor_success()
        {
            var info = typeof(TestItem).GetProperty("ItemId");
            var map = new MapItem(info);
            var item = new TestItem();

            map.Setter.Set(item, 10);

            Assert.That(map.Alias, Is.EqualTo("itemid"));
            Assert.That(map.ContentType, Is.EqualTo(typeof(int)));
            Assert.That(map.Setter.Name, Is.EqualTo("ItemId"));
            Assert.That(item.ItemId, Is.EqualTo(10));
        }

        [Test]
        public void MatchAlias_success()
        {
            var info = typeof(TestItem).GetProperty("ItemId");
            var map = new MapItem(info);

            Assert.That(map.MatchAlias("itemId"), Is.True);
            Assert.That(map.MatchAlias("itemid"), Is.True);
        }

        [Test]
        public void Constructor_type_success()
        {
            var map = new MapItem<TestItem, int>("Id", x => x.ItemId);

            Assert.That(map.Alias, Is.EqualTo("id"));
            Assert.That(map.ContentType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void Convert_success()
        {
            var map = new MapItem<TestItem, int>("Id", x => x.ItemId)
            {
                Convert = x => x.Length
            };

            var result = map.Converter("test");
            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        public async Task ActDefer_success()
        {
            var map = new MapItem<TestItem, int>("Id", x => x.ItemId)
            {
                ActDefer = async (cache, x, y) => await Task.Run(() => x.ItemId = y)
            };

            var item = new TestItem();
            await map.Defer(null, item, 3);

            Assert.That(item.ItemId, Is.EqualTo(3));
        }
    }
}
