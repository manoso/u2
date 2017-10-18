using System.Linq;
using NUnit.Framework;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class MapTaskTest
    {
        [Test]
        public void Constructor_success()
        {
            var map = new MapTask<TestItem>();
            
            Assert.That(map.Alias, Is.EqualTo("testitem"));
            Assert.That(map.EntityType, Is.EqualTo(typeof(TestItem)));
        }

        [Test]
        public void All_success()
        {
            var map = new MapTask<TestItem>();
            map.All();

            Assert.That(map.Maps.Count, Is.EqualTo(5));
        }

        [Test]
        public void Create_new_success()
        {
            var map = new MapTask<TestItem>();
            var result = map.Create();

            Assert.That(result is TestItem);
        }

        [Test]
        public void Create_with_instance()
        {
            var map = new MapTask<TestItem>();
            var instance = new TestItem { ItemId = 100};
            var result = map.Create(instance) as TestItem;

            Assert.That(result, Is.EqualTo(instance));
            Assert.That(result?.ItemId, Is.EqualTo(100));
        }


        [Test]
        public void Create_return_null()
        {
            var map = new MapTask<TestItem>();
            var instance = new TestEntity();
            var result = map.Create(instance);

            Assert.That(result is TestItem);
        }

        [Test]
        public void AliasTo_success()
        {
            var map = new MapTask<TestItem>()
                .AliasTo("ABC");
            Assert.That(map.Alias, Is.EqualTo("abc"));
        }

        [Test]
        public void AliasTo_null()
        {
            var map = new MapTask<TestItem>()
                .AliasTo(null);
            Assert.That(map.Alias, Is.EqualTo("testitem"));
        }

        [Test]
        public void Map_default()
        {
            var map = new MapTask<TestItem>()
                .Map(x => x.ItemId);

            Assert.That(map.Maps.Count, Is.EqualTo(1));
            var mapItem = map.Maps.First();
            Assert.That(mapItem.Alias, Is.EqualTo("itemid"));
            Assert.That(mapItem.Setter, Is.Not.Null);
            var item = new TestItem();
            mapItem.Setter.Set(item, 8);
            Assert.That(item.ItemId, Is.EqualTo(8));
        }

        [Test]
        public void Map_null()
        {
            var map = new MapTask<TestItem>()
                .Map<int>(null);

            Assert.That(map.Maps.Count, Is.EqualTo(0));
        }

        [Test]
        public void Map_alias()
        {
            var map = new MapTask<TestItem>()
                .Map(x => x.ItemId, "iD");

            var mapItem = map.Maps.First();
            Assert.That(mapItem.Alias, Is.EqualTo("id"));
        }

        [Test]
        public void Map_func()
        {
            var map = new MapTask<TestItem>()
                .Map(x => x.ItemId, mapFunc: x => int.Parse(x) * 2);

            var mapItem = map.Maps.First();
            var item = new TestItem();
            var value = mapItem.Converter("3");
            mapItem.Setter.Set(item, value);
            Assert.That(item.ItemId, Is.EqualTo(6));
        }
    }
}
