using System.Collections.Generic;
using NUnit.Framework;
using u2.Test;

namespace u2.Umbraco.Test
{
    [TestFixture]
    public class CmsHelperTest
    {
        [Test]
        public void Split_success()
        {
            var source = "1,2";
            var separators = new[] {','};
            var result = source.Split<int>(separators);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(1));
            Assert.That(result[1], Is.EqualTo(2));
        }

        [Test]
        public void Split_source_empty()
        {
            var source = string.Empty;
            var separators = new[] { ',' };
            var result = source.Split<int>(separators);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Split_source_empty_default()
        {
            var source = string.Empty;
            var separators = new[] { ',' };
            var defaultValue = new List<int>();
            var result = source.Split<int>(separators, defaultValue);

            Assert.That(result, Is.EqualTo(defaultValue));
        }

        [Test]
        public void Split_source_empty_entry()
        {
            var source = "1, ,2";
            var separators = new[] { ',' };
            var result = source.Split<int>(separators);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(1));
            Assert.That(result[1], Is.EqualTo(2));
        }

        [Test]
        public void JsonTo_success()
        {
            var source = "{ItemId: 1}";
            var result = source.JsonTo<TestItem>();

            Assert.That(result.ItemId, Is.EqualTo(1));
        }

        [Test]
        public void JsonTo_source_empty()
        {
            var source = string.Empty;
            var result = source.JsonTo<TestItem>();

            Assert.That(result, Is.Null);
        }

        [Test]
        public void JsonTo_source_empty_default()
        {
            var source = string.Empty;
            var defaultValue = new TestItem();
            var result = source.JsonTo(defaultValue);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(defaultValue));
        }

        [Test]
        public void JsonTo_throw()
        {
            var source = "not json";
            var result = source.JsonTo<TestItem>();

            Assert.That(result, Is.Null);
        }
    }
}
