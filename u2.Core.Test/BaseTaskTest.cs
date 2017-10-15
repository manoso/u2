using System.Linq;
using NUnit.Framework;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class BaseTaskTest
    {
        [Test]
        public void AddMap_success()
        {
            var task = new BaseTask();
            task.AddMap(new MapItem());

            Assert.That(task.Maps.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddMap_null()
        {
            var task = new BaseTask();
            task.AddMap(null);

            Assert.That(task.Maps.Count, Is.EqualTo(0));
        }

        [Test]
        public void Map_success()
        {
            var task = new BaseTask<TestItem>();
            task.Map(x => x.ItemId);
            var map = task.Maps.First();

            Assert.That(task.Maps.Count, Is.EqualTo(1));
            Assert.That(map.Alias, Is.EqualTo("itemid"));
            Assert.That(map.Setter, Is.Not.Null);
        }

        [Test]
        public void Map_with_alias_success()
        {
            var task = new BaseTask<TestItem>();
            task.Map(x => x.ItemId, "id");
            var map = task.Maps.First();

            Assert.That(map.Alias, Is.EqualTo("id"));
        }

        [Test]
        public void Map_with_func_success()
        {
            var task = new BaseTask<TestItem>();
            task.Map(x => x.ItemId, null, x => x.Length);
            var map = task.Maps.First();

            Assert.That(map.Converter, Is.Not.Null);
            Assert.That(map.Setter, Is.Not.Null);
        }

        [Test]
        public void Map_with_default_success()
        {
            var task = new BaseTask<TestItem>();
            task.Map(x => x.ItemId, null, null, 1);
            var map = task.Maps.First();

            Assert.That(map.Default, Is.EqualTo(1));
        }
    }
}
