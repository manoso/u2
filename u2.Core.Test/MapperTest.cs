using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;
using u2.Umbraco;

namespace u2.Core.Test
{
    [TestFixture]
    public class MapperTest
    {
        private IMapRegistry _rego;
        private IMapper _mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            _rego = new MapRegistry();
            _mapper = new Mapper(_rego);

            _rego.Copy<CmsKey>()
                .Map(x => x.Key);
            _rego.Copy<Model>()
                .Map(x => x.Name, "nodeName");

            _rego.Register<TestItem>();
            _rego.Register<TestEntity>()
                .Map(x => x.Name, "contentName")
                .Map(x => x.Infos, "list", x => x.Split<string>(new [] {','}))
                .MatchMany(x => x.Items);
            _rego.Register<TestAction>()
                .MapAction<int>((x, v) =>
                {
                    x.Mix = x.ActionId + v;
                }, "value1")
                .MapAction<int>((x, v1) =>
                {
                    x.Sum = v1;
                }, "value1")
                .MapAction<int, int>((x, v1, v2) =>
                {
                    x.Sum2 = v1 + v2;
                }, "value1", "value2")
                .MapAction<int, int, short>((x, v1, v2, v3) =>
                {
                    x.Sum3 = v1 + v2 + v3;
                }, "value1", "value2", "value3")
                .MapAction<int, int, short, short>((x, v1, v2, v3, v4) =>
                {
                    x.Sum4 = v1 + v2 + v3 + v4;
                }, "value1", "value2", "value3", "value4")
                .MapAction<int, int, short, short, int>((x, v1, v2, v3, v4, v5) =>
                {
                    x.Sum5 = v1 + v2 + v3 + v4 + v5;
                }, "value1", "value2", "value3", "value4", "value5")
                .MapAction<int, int, short, short, int, string>((x, v1, v2, v3, v4, v5, v6) =>
                {
                    x.Sum6 = v1 + v2 + v3 + v4 + v5 + (string.IsNullOrWhiteSpace(v6) ? 0 : int.Parse(v6));
                }, "value1", "value2", "value3", "value4", "value5", "value6")
                .MapContent((c, x) =>
                {
                    x.Agregate = c.Get<string>("value1")
                                 + c.Get<string>("value2")
                                 + c.Get<string>("value3")
                                 + c.Get<string>("value4")
                                 + c.Get<string>("value5")
                                 + c.Get<string>("value6");
                })
                ;
        }

        [Test]
        public async Task ToAsync_single_success()
        {
            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"contentName", "test name"},
                {"list", "a,b,c"},
                {"items", "1,3"}
            };
            var content = new UmbracoContent(fields);
            var result = await _mapper.ToAsync<TestEntity>(content);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1000));
            Assert.That(result.Name, Is.EqualTo("test name"));
            Assert.That(result.Infos.Count, Is.EqualTo(3));
        }

        [Test]
        public void To_single_success()
        {
            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"contentName", "test name"},
                {"list", "a,b,c"},
                {"items", "1,3"}
            };
            var content = new UmbracoContent(fields);
            var result = _mapper.To<TestEntity>(content);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1000));
            Assert.That(result.Name, Is.EqualTo("test name"));
            Assert.That(result.Infos.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task ToAsync_single_with_match_success()
        {
            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"list", "a,b,c"},
                {"items", "1,3"}
            };
            var content = new UmbracoContent(fields);
            var result = new TestEntity { Name = "name" };
            await _mapper.ToAsync(content, result);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1000));
            Assert.That(result.Name, Is.EqualTo("name"));
            Assert.That(result.Infos.Count, Is.EqualTo(3));
        }

        [Test]
        public void To_single_with_match_success()
        {
            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"list", "a,b,c"},
                {"items", "1,3"}
            };
            var content = new UmbracoContent(fields);
            var result = new TestEntity {Name = "name"};
            _mapper.To(content, result);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1000));
            Assert.That(result.Name, Is.EqualTo("name"));
            Assert.That(result.Infos.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task ToAsync_many_success()
        {
            var item1 = new Dictionary<string, string>
            {
                {"alias", "testItem"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"}
            };
            var content1 = new UmbracoContent(item1);

            var item2 = new Dictionary<string, string>
            {
                {"alias", "testItem"},
                {"itemId", "2"},
                {"price", "20.00"},
                {"onSale", "true"}
            };
            var content2 = new UmbracoContent(item2);

            var item3 = new Dictionary<string, string>
            {
                {"alias", "testItem"},
                {"itemId", "3"},
                {"price", "30.00"},
                {"onSale", "true"}
            };
            var content3 = new UmbracoContent(item3);

            var itemContents = new IContent[] { content1, content2, content3 };

            var items = (await _mapper.ToAsync<TestItem>(itemContents)).ToList();

            Assert.That(items, Is.Not.Null);
            Assert.That(items.Count, Is.EqualTo(3));
            Assert.That(items[0].ItemId, Is.EqualTo(1));
            Assert.That(items[1].ItemId, Is.EqualTo(2));
            Assert.That(items[2].ItemId, Is.EqualTo(3));
        }

        [Test]
        public void To_many_success()
        {
            var item1 = new Dictionary<string, string>
            {
                {"alias", "testItem"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"}
            };
            var content1 = new UmbracoContent(item1);

            var item2 = new Dictionary<string, string>
            {
                {"alias", "testItem"},
                {"itemId", "2"},
                {"price", "20.00"},
                {"onSale", "true"}
            };
            var content2 = new UmbracoContent(item2);

            var item3 = new Dictionary<string, string>
            {
                {"alias", "testItem"},
                {"itemId", "3"},
                {"price", "30.00"},
                {"onSale", "true"}
            };
            var content3 = new UmbracoContent(item3);

            var itemContents = new IContent[] { content1, content2, content3 };

            var items = _mapper.To<TestItem>(itemContents).ToList();

            Assert.That(items, Is.Not.Null);
            Assert.That(items.Count, Is.EqualTo(3));
            Assert.That(items[0].ItemId, Is.EqualTo(1));
            Assert.That(items[1].ItemId, Is.EqualTo(2));
            Assert.That(items[2].ItemId, Is.EqualTo(3));
        }

        [Test]
        public async Task ToAsync_many_with_match_success()
        {
            var item1 = new Dictionary<string, string>
            {
                {"alias", "testItem"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"}
            };
            var content1 = new UmbracoContent(item1);

            var item2 = new Dictionary<string, string>
            {
                {"alias", "testItem"},
                {"itemId", "2"},
                {"price", "20.00"},
                {"onSale", "true"}
            };
            var content2 = new UmbracoContent(item2);

            var item3 = new Dictionary<string, string>
            {
                {"alias", "testItem"},
                {"itemId", "3"},
                {"price", "30.00"},
                {"onSale", "true"}
            };
            var content3 = new UmbracoContent(item3);

            var itemContents = new IContent[] { content1, content2, content3 };

            var items = new[]
            {
                new TestItem {ItemId = 1, Name = "one"},
                new TestItem {ItemId = 2, Name = "two"},
                new TestItem {ItemId = 3, Name = "three"},
            };

            (await _mapper.ToAsync(itemContents, items, x => x.ItemId, "itemId")).ToList();

            Assert.That(items, Is.Not.Null);
            Assert.That(items.Count, Is.EqualTo(3));
            //load from content
            Assert.That(items[0].Price, Is.EqualTo(10d));
            Assert.That(items[1].Price, Is.EqualTo(20d));
            Assert.That(items[2].Price, Is.EqualTo(30d));
            //existing value
            Assert.That(items[0].Name, Is.EqualTo("one"));
            Assert.That(items[1].Name, Is.EqualTo("two"));
            Assert.That(items[2].Name, Is.EqualTo("three"));
        }

        [Test]
        public void To_many_with_match_success()
        {
            var item1 = new Dictionary<string, string>
            {
                {"alias", "testItem"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"}
            };
            var content1 = new UmbracoContent(item1);

            var item2 = new Dictionary<string, string>
            {
                {"alias", "testItem"},
                {"itemId", "2"},
                {"price", "20.00"},
                {"onSale", "true"}
            };
            var content2 = new UmbracoContent(item2);

            var item3 = new Dictionary<string, string>
            {
                {"alias", "testItem"},
                {"itemId", "3"},
                {"price", "30.00"},
                {"onSale", "true"}
            };
            var content3 = new UmbracoContent(item3);

            var itemContents = new IContent[] { content1, content2, content3 };

            var items = new[]
            {
                new TestItem {ItemId = 1, Name = "one"},
                new TestItem {ItemId = 2, Name = "two"},
                new TestItem {ItemId = 3, Name = "three"},
            };

            _mapper.To(itemContents, items, x => x.ItemId, "itemId").ToList();

            Assert.That(items, Is.Not.Null);
            Assert.That(items.Count, Is.EqualTo(3));
            //load from content
            Assert.That(items[0].Price, Is.EqualTo(10d));
            Assert.That(items[1].Price, Is.EqualTo(20d));
            Assert.That(items[2].Price, Is.EqualTo(30d));
            //existing value
            Assert.That(items[0].Name, Is.EqualTo("one"));
            Assert.That(items[1].Name, Is.EqualTo("two"));
            Assert.That(items[2].Name, Is.EqualTo("three"));
        }

        [Test]
        public async Task ToAsync_Act_1_augs_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = await _mapper.ToAsync<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Sum, Is.EqualTo(1));
        }

        [Test]
        public void To_Act_1_augs_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = _mapper.To<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Sum, Is.EqualTo(1));
        }

        [Test]
        public async Task ToAsync_Act_2_augs_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = await _mapper.ToAsync<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Sum2, Is.EqualTo(3));
        }

        [Test]
        public void To_Act_2_augs_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = _mapper.To<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Sum2, Is.EqualTo(3));
        }

        [Test]
        public async Task ToAsync_Act_3_augs_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = await _mapper.ToAsync<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Sum3, Is.EqualTo(6));
        }

        [Test]
        public void To_Act_3_augs_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = _mapper.To<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Sum3, Is.EqualTo(6));
        }

        [Test]
        public async Task ToAsync_Act_4_augs_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = await _mapper.ToAsync<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Sum4, Is.EqualTo(10));
        }

        [Test]
        public void To_Act_4_augs_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = _mapper.To<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Sum4, Is.EqualTo(10));
        }

        [Test]
        public async Task ToAsync_Act_5_augs_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = await _mapper.ToAsync<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Sum5, Is.EqualTo(15));
        }

        [Test]
        public void To_Act_5_augs_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = _mapper.To<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Sum5, Is.EqualTo(15));
        }

        [Test]
        public async Task ToAsync_Act_6_augs_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = await _mapper.ToAsync<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Sum6, Is.EqualTo(21));
        }

        [Test]
        public void To_Act_6_augs_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = _mapper.To<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Sum6, Is.EqualTo(21));
        }

        [Test]
        public async Task ToAsync_Act_mix_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = await _mapper.ToAsync<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Mix, Is.EqualTo(2));
        }

        [Test]
        public void To_Act_mix_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };
            var content = new UmbracoContent(fields);
            var result = _mapper.To<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Mix, Is.EqualTo(2));
        }

        [Test]
        public async Task ToAsync_Act_Content_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };

            var content = new UmbracoContent(fields);
            var result = await _mapper.ToAsync<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Agregate, Is.EqualTo("123456"));
        }

        [Test]
        public void To_Act_Content_Success()
        {
            var fields = new Dictionary<string, string>
            {
                {"actionid", "1"},
                {"name", "test action"},
                {"value1", "1"},
                {"value2", "2"},
                {"value3", "3"},
                {"value4", "4"},
                {"value5", "5"},
                {"value6", "6"}
            };

            var content = new UmbracoContent(fields);
            var result = _mapper.To<TestAction>(content);

            Assert.IsNotNull(result);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.Agregate, Is.EqualTo("123456"));
        }

        [Test]
        public async Task ToAsync_Copy_Success()
        {
            var guid = Guid.NewGuid().ToString("N");
            var fields = new Dictionary<string, string>
            {
                {"key", guid },
                {"id", "cmskey" },
                {"nodeName", "a name" }
            };

            var content = new UmbracoContent(fields);
            var item = await  _mapper.ToAsync<TestItem>(content);
            var action = await _mapper.ToAsync<TestAction>(content);

            Assert.IsNotNull(item);
            Assert.That(item.Key.ToString("N"), Is.EqualTo(guid));
            Assert.That(item.Name, Is.EqualTo("a name"));

            Assert.IsNotNull(action);
            Assert.That(action.Key.ToString("N"), Is.EqualTo(guid));
            Assert.That(action.Name, Is.Null);
        }

        [Test]
        public void To_Copy_Success()
        {
            var guid = Guid.NewGuid().ToString("N");
            var fields = new Dictionary<string, string>
            {
                {"key", guid },
                {"id", "cmskey" },
                {"nodeName", "a name" }
            };

            var content = new UmbracoContent(fields);
            var item = _mapper.To<TestItem>(content);
            var action = _mapper.To<TestAction>(content);

            Assert.IsNotNull(item);
            Assert.That(item.Key.ToString("N"), Is.EqualTo(guid));
            Assert.That(item.Name, Is.EqualTo("a name"));

            Assert.IsNotNull(action);
            Assert.That(action.Key.ToString("N"), Is.EqualTo(guid));
            Assert.That(action.Name, Is.Null);
        }
    }

}
