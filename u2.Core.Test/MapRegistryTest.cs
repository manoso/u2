using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class MapRegistryTest
    {
        [Test]
        public void Register_has()
        {
            var registry = new MapRegistry();

            registry.Register<TestItem>();
            Assert.That(registry.Has(typeof(TestItem)));
        }

        [Test]
        public void Register_Indexer()
        {
            var registry = new MapRegistry();

            registry.Register<TestItem>();
            Assert.That(registry[typeof(TestItem)], Is.Not.Null);
        }

        [Test]
        public void Register_GetType_return_type()
        {
            var registry = new MapRegistry();

            registry.Register<TestItem>().AliasTo("testObj");
            Assert.That(registry.GetType("TestObj"), Is.EqualTo(typeof(TestItem)));
        }

        [Test]
        public void Register_GetType_return_null()
        {
            var registry = new MapRegistry();

            registry.Register<TestItem>().AliasTo("testObj");
            Assert.That(registry.GetType("TestOb"), Is.Null);
        }

        [Test]
        public void Register_Map_All()
        {
            var registry = new MapRegistry();

            var task = registry.Register<TestItem>();
            Assert.That(task, Is.Not.Null);
            Assert.That(task.Maps.Count, Is.EqualTo(5));
        }

        [Test]
        public void Register_Copy_Map()
        {
            var registry = new MapRegistry();

            registry.Copy<CmsKey>()
                .Map(x => x.Key);

            var task = registry.Register<TestItem>();
            Assert.That(task, Is.Not.Null);
            Assert.That(task.Maps.Count, Is.EqualTo(6));
            Assert.That(task.Maps[5] is MapItem<CmsKey, string>);
        }
    }
}
