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
    }
}
