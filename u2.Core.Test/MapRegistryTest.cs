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
        public void Constructor_has_root()
        {
            var root = Substitute.For<IRoot>();
            var registry = new MapRegistry(root);

            Assert.That(registry.Root, Is.EqualTo(root));
        }

        [Test]
        public void Register_has()
        {
            var root = Substitute.For<IRoot>();
            var registry = new MapRegistry(root);

            registry.Register<TestItem>();
            Assert.That(registry.Has(typeof(TestItem)));
        }

        [Test]
        public void Register_Indexer()
        {
            var root = Substitute.For<IRoot>();
            var registry = new MapRegistry(root);

            registry.Register<TestItem>();
            Assert.That(registry[typeof(TestItem)], Is.Not.Null);
        }

        [Test]
        public void Register_GetType_return_type()
        {
            var root = Substitute.For<IRoot>();
            var registry = new MapRegistry(root);

            registry.Register<TestItem>().AliasTo("testObj");
            Assert.That(registry.GetType("TestObj"), Is.EqualTo(typeof(TestItem)));
        }

        [Test]
        public void Register_GetType_return_null()
        {
            var root = Substitute.For<IRoot>();
            var registry = new MapRegistry(root);

            registry.Register<TestItem>().AliasTo("testObj");
            Assert.That(registry.GetType("TestOb"), Is.Null);
        }

        [Test]
        public void Register_Map_All()
        {
            var root = Substitute.For<IRoot>();
            var registry = new MapRegistry(root);

            var task = registry.Register<TestItem>();
            Assert.That(task, Is.Not.Null);
            Assert.That(task.Maps.Count, Is.EqualTo(5));
        }

        [Test]
        public void Register_Copy_Map()
        {
            var root = Substitute.For<IRoot>();
            var registry = new MapRegistry(root);

            registry.Copy<CmsKey>()
                .Map(x => x.Key);

            var task = registry.Register<TestItem>();
            Assert.That(task, Is.Not.Null);
            Assert.That(task.Maps.Count, Is.EqualTo(5));
            Assert.That(task.Maps[4] is MapItem<CmsKey, string>);
        }
    }
}
