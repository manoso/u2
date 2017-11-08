using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using u2.Core;
using u2.Core.Contract;
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

        [Test]
        public void Archetype_success()
        {
            var source = @"{
fieldsets: [
    {properties: [
        {alias: 'itemid', value: 1},
        {alias: 'price', value: .5}
    ]},
    {properties: [
        {alias: 'itemid', value: 2},
        {alias: 'price', value: 1.5}
    ]}
]}";
            var root = Substitute.For<IRoot>();
            var rego = new MapRegistry(root);
            var mapper = new Mapper(rego);

            rego.Register<TestItem>();

            var result = source.Archetype<TestItem>(mapper);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].ItemId, Is.EqualTo(1));
            Assert.That(result[0].Price, Is.EqualTo(0.5));
            Assert.That(result[1].ItemId, Is.EqualTo(2));
            Assert.That(result[1].Price, Is.EqualTo(1.5));
        }

        [Test]
        public void Archetype_fieldsets_empty_return_null()
        {
            var source = @"{fieldsets: []}";
            var root = Substitute.For<IRoot>();
            var rego = new MapRegistry(root);
            var mapper = new Mapper(rego);

            rego.Register<TestItem>();

            var result = source.Archetype<TestItem>(mapper);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Archetype_source_empty()
        {
            var source = string.Empty;
            var result = source.Archetype<TestItem>(null);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Archetype_source_empty_default()
        {
            var source = string.Empty;
            var defaultValue = new List<TestItem>();
            var result = source.Archetype(null, defaultValue);

            Assert.That(result, Is.EqualTo(defaultValue));
        }
    }
}
