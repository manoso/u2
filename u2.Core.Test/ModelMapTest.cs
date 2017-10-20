using System.Linq;
using NUnit.Framework;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class ModelMapTest
    {
        [Test]
        public void Constructor_many()
        {
            var map = new ModelMap<TestEntity, TestItem>("Id", (x, y) => x.Items = y);

            Assert.That(map.SetModel, Is.Not.Null);
            Assert.That(map.Alias, Is.EqualTo("id"));
            Assert.That(map.ModelType, Is.EqualTo(typeof(TestItem)));
            Assert.That(map.IsMany);
            Assert.That(map.GetKey, Is.EqualTo(ModelMap.DefaultGetKey));
        }

        [Test]
        public void Constructor_single()
        {
            var map = new ModelMap<TestEntity, TestItem>("Id", (x, y) => x.Item = y);

            Assert.That(map.SetModel, Is.Not.Null);
            Assert.That(map.Alias, Is.EqualTo("id"));
            Assert.That(map.ModelType, Is.EqualTo(typeof(TestItem)));
            Assert.That(!map.IsMany);
            Assert.That(map.GetKey, Is.EqualTo(ModelMap.DefaultGetKey));
        }

        [Test]
        public void Match_many_default()
        {
            var map = new ModelMap<TestEntity, TestItem>("items", (x, y) => x.Items = y);
            var entity = new TestEntity();
            var items = new[]
            {
                new TestItem {Key = "1"},
                new TestItem {Key = "2"},
                new TestItem {Key = "3"}
            };

            map.Match(entity, new [] {"1","2"}, items);
            Assert.That(entity.Items.Count(), Is.EqualTo(2));
            Assert.That(entity.Items.First().Key, Is.EqualTo("1"));
            Assert.That(entity.Items.Skip(1).Take(1).First().Key, Is.EqualTo("2"));
        }

        [Test]
        public void Match_many_keyFunc()
        {
            var map = new ModelMap<TestEntity, TestItem>("items", (x, y) => x.Items = y, x => x.ItemId.ToString());
            var entity = new TestEntity();
            var items = new[]
            {
                new TestItem {ItemId = 1},
                new TestItem {ItemId = 2},
                new TestItem {ItemId = 3}
            };

            map.Match(entity, new[] { "1", "3" }, items);
            Assert.That(entity.Items.Count(), Is.EqualTo(2));
            Assert.That(entity.Items.First().ItemId, Is.EqualTo(1));
            Assert.That(entity.Items.Skip(1).Take(1).First().ItemId, Is.EqualTo(3));
        }

        [Test]
        public void Match_single_default()
        {
            var map = new ModelMap<TestEntity, TestItem>("item", (x, y) => x.Item = y);
            var entity = new TestEntity();
            var items = new[]
            {
                new TestItem {Key = "1"},
                new TestItem {Key = "2"},
                new TestItem {Key = "3"}
            };

            map.Match(entity, "2", items);
            Assert.That(entity.Item, Is.Not.Null);
            Assert.That(entity.Item.Key, Is.EqualTo("2"));
        }

        [Test]
        public void Match_single_keyFunc()
        {
            var map = new ModelMap<TestEntity, TestItem>("item", (x, y) => x.Item = y, x => x.ItemId.ToString());
            var entity = new TestEntity();
            var items = new[]
            {
                new TestItem {ItemId = 1},
                new TestItem {ItemId = 2},
                new TestItem {ItemId = 3}
            };

            map.Match(entity, "3", items);
            Assert.That(entity.Item, Is.Not.Null);
            Assert.That(entity.Item.ItemId, Is.EqualTo(3));
        }
    }
}