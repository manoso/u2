
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class TaskDeferTest
    {
        [Test]
        public void Constructor_with_model_maps_success()
        {
            var mapTask = new MapTask<TestEntity>();
            mapTask.Match(x => x.Item);
            var defer = new TaskDefer(mapTask.ModelMaps);

            Assert.That(defer.Maps, Is.Not.Null);
            Assert.That(defer.Maps.Count, Is.EqualTo(1));
            Assert.That(defer.Maps.First() is MapItem<object, string>);
        }

        [Test]
        public void Constructor_without_model_maps_success()
        {
            var mapTask = new MapTask<TestEntity>();
            var defer = new TaskDefer(mapTask.ModelMaps);

            Assert.That(defer.Maps, Is.Not.Null);
            Assert.That(defer.Maps.Count, Is.EqualTo(0));
        }
    }
}
