using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using u2.Core;
using u2.Test;
using u2.Umbraco.DataType;

namespace u2.Umbraco.DataType.Test
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
            var result = source.Split(separators, defaultValue);

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
        public async Task ToArchetypes_success()
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
            var rego = new MapRegistry();
            var mapper = new Mapper(rego);

            rego.Register<TestItem>();

            var result = await source.ToArchetypes<TestItem>()(mapper, null) as IList<TestItem>;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].ItemId, Is.EqualTo(1));
            Assert.That(result[0].Price, Is.EqualTo(0.5));
            Assert.That(result[1].ItemId, Is.EqualTo(2));
            Assert.That(result[1].Price, Is.EqualTo(1.5));
        }

        [Test]
        public async Task ToArchetypes_fieldsets_empty_return_null()
        {
            var source = @"{fieldsets: []}";
            var rego = new MapRegistry();
            var mapper = new Mapper(rego);

            rego.Register<TestItem>();

            var result = await source.ToArchetypes<TestItem>()(mapper, null) as IList<TestItem>;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ToArchetypes_source_empty_return_null()
        {
            var source = string.Empty;
            var result = await source.ToArchetypes<TestItem>()(null, null) as IList<TestItem>;

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task ToArchetypes_source_empty_default()
        {
            var source = string.Empty;
            var defaultValue = new List<TestItem>();
            var result = (await source.ToArchetypes(defaultValue)(null, null)) as IList<TestItem>;

            Assert.That(result, Is.EqualTo(defaultValue));
        }

        [Test]
        public async Task ToNestedContents_success()
        {
            var source = @"[
{
    'key':'ae2df017-bc64-4d0c-8612-3e1414c3f98f',
    'name':'Item 1',
    'ncContentTypeAlias':'imageGrid',
    'price':'.5',
    'itemid':'1'
},
{
    'key':'49706a5a-2d2d-4f55-8702-c41b553d6aa0',
    'name':'Item 2',
    'ncContentTypeAlias':'info',
    'price':'1.5',
    'itemid':'2'
}]";
            var rego = new MapRegistry();
            var mapper = new Mapper(rego);

            rego.Register<TestItem>();

            var result = await source.ToNestedContents<TestItem>()(mapper, null) as IList<TestItem>;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].ItemId, Is.EqualTo(1));
            Assert.That(result[0].Price, Is.EqualTo(0.5));
            Assert.That(result[1].ItemId, Is.EqualTo(2));
            Assert.That(result[1].Price, Is.EqualTo(1.5));
        }

        [Test]
        public async Task ToNestedContents_empty_return_null()
        {
            var source = @"[]";
            var rego = new MapRegistry();
            var mapper = new Mapper(rego);

            rego.Register<TestItem>();

            var result = await source.ToNestedContents<TestItem>()(mapper, null) as IList<TestItem>;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ToNestedContents_source_empty_return_null()
        {
            var source = string.Empty;
            var result = await source.ToNestedContents<TestItem>()(null, null) as IList<TestItem>;

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task ToNestedContents_source_empty_default()
        {
            var source = string.Empty;
            var defaultValue = new List<TestItem>();
            var result = (await source.ToNestedContents(defaultValue)(null, null)) as IList<TestItem>;

            Assert.That(result, Is.EqualTo(defaultValue));
        }
    }
}
