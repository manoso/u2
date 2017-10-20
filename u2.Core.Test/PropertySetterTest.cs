using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class PropertySetterTest
    {
        [Test]
        public void Constructor_success()
        {
            var info = typeof(TestItem).GetProperty("ItemId");
            var setter = new PropertySetter(info);
            var item = new TestItem();
            setter.Set(item, 1);

            Assert.That(setter.Name, Is.EqualTo("ItemId"));
            Assert.That(setter.Set, Is.Not.Null);
            Assert.That(item.ItemId, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_null()
        {
            var setter = new PropertySetter(null);

            Assert.That(setter.Name, Is.Null);
            Assert.That(setter.Set, Is.Null);
        }

        [Test]
        public void Constructor_expression()
        {
            var setter = new PropertySetter<TestItem, int>(x => x.ItemId);
            var item = new TestItem();
            setter.Set(item, 1);

            Assert.That(setter.Name, Is.EqualTo("ItemId"));
            Assert.That(setter.Set, Is.Not.Null);
            Assert.That(item.ItemId, Is.EqualTo(1));
        }
    }
}
