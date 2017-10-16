using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class MapDeferTest
    {
        [Test]
        public void For_success()
        {
            var mapDefer = new MapDefer();

            var result = mapDefer.For<TestItem>();
            var target = mapDefer.Defers[typeof(TestItem)];

            Assert.That(result, Is.EqualTo(target));
        }

        [Test]
        public void For_type_success()
        {
            var mapDefer = new MapDefer();
            var type = typeof(TestItem);

            var result = mapDefer.For(type);
            var target = mapDefer.Defers[type];

            Assert.That(result, Is.EqualTo(target));
        }

        [Test]
        public void For_existing_not_added()
        {
            var mapDefer = new MapDefer();
            var type = typeof(TestItem);

            var current = mapDefer.For<TestItem>();
            var result = mapDefer.For(type);
            var target = mapDefer.Defers[type];

            Assert.That(current, Is.EqualTo(target));
            Assert.That(result, Is.EqualTo(current));
        }
        [Test]
        public void Defer_null_return()
        {
            var type = typeof(TestItem);
            var mapTask = Substitute.For<IMapTask>();

            mapTask.EntityType.Returns(type);

            var mapDefer = new MapDefer();

            mapDefer.Defer(mapTask, null);

            mapTask.ModelMaps.Received(0);
        }

        [Test]
        public void Defer_direct_return()
        {
            var type = typeof(TestItem);
            var mapTask = Substitute.For<IMapTask>();

            async Task<IEnumerable<object>> TaskGet(Type t, string key = null)
            {
                return await Task.Run(() => null as IEnumerable<object>);
            }

            mapTask.EntityType.Returns(type);

            var mapDefer = new MapDefer();
            mapDefer.For<TestItem>();

            mapDefer.Defer(mapTask, TaskGet);

            mapTask.ModelMaps.Received(0);
        }

        [Test]
        public void Defer_attach_called()
        {
            var type = typeof(TestItem);
            var mapTask = Substitute.For<IMapTask>();
            var modelMap = Substitute.For<IModelMap>();
            async Task<IEnumerable<object>> TaskGet(Type t, string key = null)
            {
                return await Task.Run(() => null as IEnumerable<object>);
            }

            mapTask.EntityType.Returns(type);
            mapTask.ModelMaps.Returns(new List<IModelMap> { modelMap });

            var mapDefer = new MapDefer();

            mapDefer.Defer(mapTask, TaskGet);

            var result = mapTask.Received(1).ModelMaps;
        }
    }
}
