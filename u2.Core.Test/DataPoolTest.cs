using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using u2.Cache;
using u2.Core.Contract;
using u2.Test;
using u2.Umbraco;

namespace u2.Core.Test
{
    [TestFixture]
    public class DataPoolTest
    {
        [Test]
        public async Task GetAsync_fit_single_success()
        {
            var root = Substitute.For<IRoot>();
            var mapRegistry = new MapRegistry(root);
            var mapper = new Mapper(mapRegistry);
            var cacheRegistry = new CacheRegistry();
            var cacheStore = new CacheStore();
            var cacheFetcher = new CacheFetcher(cacheStore, cacheRegistry);
            var queryFactory = Substitute.For<IQueryFactory>();
            var cmsFetcher = Substitute.For<ICmsFetcher>();

            mapRegistry.Copy<CmsKey>()
                .Map(x => x.Key, "id");
            mapRegistry.Copy<Model>()
                .Map(x => x.Name, "alias");

            mapRegistry.Register<TestItem>()
                .Map(x => x.Key, "itemId");
            mapRegistry.Register<TestEntity>()
                .Fit(x => x.Item);

            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"list", "a,b,c"},
                {"item", "2"}
            };
            var content = new UmbracoContent(fields);

            var item1 = new Dictionary<string, string>
            {
                {"alias", "testItem1"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"}
            };
            var content1 = new UmbracoContent(item1);

            var item2 = new Dictionary<string, string>
            {
                {"alias", "testItem2"},
                {"itemId", "2"},
                {"price", "20.00"},
                {"onSale", "true"}
            };
            var content2 = new UmbracoContent(item2);

            var item3 = new Dictionary<string, string>
            {
                {"alias", "testItem3"},
                {"itemId", "3"},
                {"price", "30.00"},
                {"onSale", "true"}
            };
            var content3 = new UmbracoContent(item3);

            var mapItem = mapRegistry.For<TestItem>();
            var mapEntity = mapRegistry.For<TestEntity>();
            var queryItem = Substitute.For<ICmsQuery>();
            var queryEntity = Substitute.For<ICmsQuery>();
            queryFactory.Create(mapItem).Returns(queryItem);
            queryFactory.Create(mapEntity).Returns(queryEntity);

            var entityContents = new IContent[] { content };
            var itemContents = new IContent[] { content1, content2, content3 };

            cmsFetcher.Fetch(queryItem).Returns(itemContents);
            cmsFetcher.Fetch(queryEntity).Returns(entityContents);

            var pool = new DataPool(mapRegistry, mapper, cacheRegistry, cacheFetcher, queryFactory, cmsFetcher);

            var entities = await pool.GetAsync<TestEntity>();
            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Item, Is.Not.Null);
            Assert.That(entity.Item.ItemId, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAsync_fit_single_with_key_success()
        {
            var root = Substitute.For<IRoot>();
            var mapRegistry = new MapRegistry(root);
            var mapper = new Mapper(mapRegistry);
            var cacheRegistry = new CacheRegistry();
            var cacheStore = new CacheStore();
            var cacheFetcher = new CacheFetcher(cacheStore, cacheRegistry);
            var queryFactory = Substitute.For<IQueryFactory>();
            var cmsFetcher = Substitute.For<ICmsFetcher>();
            var registry = new Registry(mapRegistry, mapper, cacheRegistry, cacheFetcher, queryFactory, cmsFetcher);

            mapRegistry.Copy<CmsKey>()
                .Map(x => x.Key, "id");
            mapRegistry.Copy<Model>()
                .Map(x => x.Name, "alias");

            registry.Register<TestItem>()
                .Map(x => x.ItemId);
            registry.Register<TestEntity>()
                .Fit(x => x.Item, x => x.ItemId.ToString());

            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"list", "a,b,c"},
                {"item", "2"}
            };
            var content = new UmbracoContent(fields);

            var item1 = new Dictionary<string, string>
            {
                {"alias", "testItem1"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"}
            };
            var content1 = new UmbracoContent(item1);

            var item2 = new Dictionary<string, string>
            {
                {"alias", "testItem2"},
                {"itemId", "2"},
                {"price", "20.00"},
                {"onSale", "true"}
            };
            var content2 = new UmbracoContent(item2);

            var item3 = new Dictionary<string, string>
            {
                {"alias", "testItem3"},
                {"itemId", "3"},
                {"price", "30.00"},
                {"onSale", "true"}
            };
            var content3 = new UmbracoContent(item3);

            var mapItem = mapRegistry.For<TestItem>();
            var mapEntity = mapRegistry.For<TestEntity>();
            var queryItem = Substitute.For<ICmsQuery>();
            var queryEntity = Substitute.For<ICmsQuery>();
            queryFactory.Create(mapItem).Returns(queryItem);
            queryFactory.Create(mapEntity).Returns(queryEntity);

            var entityContents = new IContent[] { content };
            var itemContents = new IContent[] { content1, content2, content3 };

            cmsFetcher.Fetch(queryItem).Returns(itemContents);
            cmsFetcher.Fetch(queryEntity).Returns(entityContents);

            var pool = new DataPool(mapRegistry, mapper, cacheRegistry, cacheFetcher, queryFactory, cmsFetcher);

            var entities = await pool.GetAsync<TestEntity>();
            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Item, Is.Not.Null);
            Assert.That(entity.Item.ItemId, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAsync_fit_many_success()
        {
            var root = Substitute.For<IRoot>();
            var mapRegistry = new MapRegistry(root);
            var mapper = new Mapper(mapRegistry);
            var cacheRegistry = new CacheRegistry();
            var cacheStore = new CacheStore();
            var cacheFetcher = new CacheFetcher(cacheStore, cacheRegistry);
            var queryFactory = Substitute.For<IQueryFactory>();
            var cmsFetcher = Substitute.For<ICmsFetcher>();

            mapRegistry.Copy<CmsKey>()
                .Map(x => x.Key, "id");
            mapRegistry.Copy<Model>()
                .Map(x => x.Name, "alias");

            mapRegistry.Register<TestItem>()
                .Map(x => x.Key, "itemId");
            mapRegistry.Register<TestEntity>()
                .Fit(x => x.Items);

            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"list", "a,b,c"},
                {"items", "1,3"},
            };
            var content = new UmbracoContent(fields);

            var item1 = new Dictionary<string, string>
            {
                {"alias", "testItem1"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"}
            };
            var content1 = new UmbracoContent(item1);

            var item2 = new Dictionary<string, string>
            {
                {"alias", "testItem2"},
                {"itemId", "2"},
                {"price", "20.00"},
                {"onSale", "true"}
            };
            var content2 = new UmbracoContent(item2);

            var item3 = new Dictionary<string, string>
            {
                {"alias", "testItem3"},
                {"itemId", "3"},
                {"price", "30.00"},
                {"onSale", "true"}
            };
            var content3 = new UmbracoContent(item3);

            var mapItem = mapRegistry.For<TestItem>();
            var mapEntity = mapRegistry.For<TestEntity>();
            var queryItem = Substitute.For<ICmsQuery>();
            var queryEntity = Substitute.For<ICmsQuery>();
            queryFactory.Create(mapItem).Returns(queryItem);
            queryFactory.Create(mapEntity).Returns(queryEntity);

            var entityContents = new IContent[] {content};
            var itemContents = new IContent[] { content1, content2, content3 };

            cmsFetcher.Fetch(queryItem).Returns(itemContents);
            cmsFetcher.Fetch(queryEntity).Returns(entityContents);

            var pool = new DataPool(mapRegistry, mapper, cacheRegistry, cacheFetcher, queryFactory, cmsFetcher);

            var entities = await pool.GetAsync<TestEntity>();
            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Items, Is.Not.Null);
            Assert.That(entity.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAsync_fit_many_with_key_success()
        {
            var root = Substitute.For<IRoot>();
            var mapRegistry = new MapRegistry(root);
            var mapper = new Mapper(mapRegistry);
            var cacheRegistry = new CacheRegistry();
            var cacheStore = new CacheStore();
            var cacheFetcher = new CacheFetcher(cacheStore, cacheRegistry);
            var queryFactory = Substitute.For<IQueryFactory>();
            var cmsFetcher = Substitute.For<ICmsFetcher>();

            mapRegistry.Copy<CmsKey>()
                .Map(x => x.Key, "id");
            mapRegistry.Copy<Model>()
                .Map(x => x.Name, "alias");

            mapRegistry.Register<TestItem>()
                .Map(x => x.Key, "itemId");
            mapRegistry.Register<TestEntity>()
                .Fit(x => x.Items, x => x.ItemId.ToString());

            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"list", "a,b,c"},
                {"items", "1,3"},
            };
            var content = new UmbracoContent(fields);

            var item1 = new Dictionary<string, string>
            {
                {"alias", "testItem1"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"}
            };
            var content1 = new UmbracoContent(item1);

            var item2 = new Dictionary<string, string>
            {
                {"alias", "testItem2"},
                {"itemId", "2"},
                {"price", "20.00"},
                {"onSale", "true"}
            };
            var content2 = new UmbracoContent(item2);

            var item3 = new Dictionary<string, string>
            {
                {"alias", "testItem3"},
                {"itemId", "3"},
                {"price", "30.00"},
                {"onSale", "true"}
            };
            var content3 = new UmbracoContent(item3);

            var mapItem = mapRegistry.For<TestItem>();
            var mapEntity = mapRegistry.For<TestEntity>();
            var queryItem = Substitute.For<ICmsQuery>();
            var queryEntity = Substitute.For<ICmsQuery>();
            queryFactory.Create(mapItem).Returns(queryItem);
            queryFactory.Create(mapEntity).Returns(queryEntity);

            var entityContents = new IContent[] { content };
            var itemContents = new IContent[] { content1, content2, content3 };

            cmsFetcher.Fetch(queryItem).Returns(itemContents);
            cmsFetcher.Fetch(queryEntity).Returns(entityContents);

            var pool = new DataPool(mapRegistry, mapper, cacheRegistry, cacheFetcher, queryFactory, cmsFetcher);

            var entities = await pool.GetAsync<TestEntity>();
            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Items, Is.Not.Null);
            Assert.That(entity.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetchAsync_fit_action_success()
        {
            var root = Substitute.For<IRoot>();
            var mapRegistry = new MapRegistry(root);
            var mapper = new Mapper(mapRegistry);
            var cacheRegistry = new CacheRegistry();
            var cacheStore = new CacheStore();
            var cacheFetcher = new CacheFetcher(cacheStore, cacheRegistry);
            var queryFactory = Substitute.For<IQueryFactory>();
            var cmsFetcher = Substitute.For<ICmsFetcher>();

            mapRegistry.Copy<CmsKey>()
                .Map(x => x.Key, "id");
            mapRegistry.Copy<Model>()
                .Map(x => x.Name, "alias");

            mapRegistry.Register<TestItem>()
                .Map(x => x.Key, "itemId");
            mapRegistry.Register<TestEntity>()
                .Fit<TestItem>((x, y) =>
                {
                    x.List = y.ToList();
                    x.Dictionary = x.List.ToDictionary(z => z.Key, z => z);
                }, "items");

            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"list", "a,b,c"},
                {"items", "1,3"},
            };
            var content = new UmbracoContent(fields);

            var item1 = new Dictionary<string, string>
            {
                {"alias", "testItem1"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"}
            };
            var content1 = new UmbracoContent(item1);

            var item2 = new Dictionary<string, string>
            {
                {"alias", "testItem2"},
                {"itemId", "2"},
                {"price", "20.00"},
                {"onSale", "true"}
            };
            var content2 = new UmbracoContent(item2);

            var item3 = new Dictionary<string, string>
            {
                {"alias", "testItem3"},
                {"itemId", "3"},
                {"price", "30.00"},
                {"onSale", "true"}
            };
            var content3 = new UmbracoContent(item3);

            var mapItem = mapRegistry.For<TestItem>();
            var mapEntity = mapRegistry.For<TestEntity>();
            var queryItem = Substitute.For<ICmsQuery>();
            var queryEntity = Substitute.For<ICmsQuery>();
            queryFactory.Create(mapItem).Returns(queryItem);
            queryFactory.Create(mapEntity).Returns(queryEntity);

            var entityContents = new IContent[] { content };
            var itemContents = new IContent[] { content1, content2, content3 };

            cmsFetcher.Fetch(queryItem).Returns(itemContents);
            cmsFetcher.Fetch(queryEntity).Returns(entityContents);

            var pool = new DataPool(mapRegistry, mapper, cacheRegistry, cacheFetcher, queryFactory, cmsFetcher);

            var entities = await pool.GetAsync<TestEntity>();
            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.List, Is.Not.Null);
            Assert.That(entity.List.Count, Is.EqualTo(2));
            Assert.That(entity.Dictionary, Is.Not.Null);
            Assert.That(entity.Dictionary.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAsync_fit_action_with_key_success()
        {
            var root = Substitute.For<IRoot>();
            var mapRegistry = new MapRegistry(root);
            var mapper = new Mapper(mapRegistry);
            var cacheRegistry = new CacheRegistry();
            var cacheStore = new CacheStore();
            var cacheFetcher = new CacheFetcher(cacheStore, cacheRegistry);
            var queryFactory = Substitute.For<IQueryFactory>();
            var cmsFetcher = Substitute.For<ICmsFetcher>();

            mapRegistry.Copy<CmsKey>()
                .Map(x => x.Key, "id");
            mapRegistry.Copy<Model>()
                .Map(x => x.Name, "alias");

            mapRegistry.Register<TestItem>()
                .Map(x => x.Key, "itemId");
            mapRegistry.Register<TestEntity>()
                .Fit<TestItem>((x, y) =>
                {
                    x.List = y.ToList();
                    x.Dictionary = x.List.ToDictionary(z => z.Key, z => z);
                }, "items", x => x.ItemId.ToString());

            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"list", "a,b,c"},
                {"items", "1,3"},
            };
            var content = new UmbracoContent(fields);

            var item1 = new Dictionary<string, string>
            {
                {"alias", "testItem1"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"}
            };
            var content1 = new UmbracoContent(item1);

            var item2 = new Dictionary<string, string>
            {
                {"alias", "testItem2"},
                {"itemId", "2"},
                {"price", "20.00"},
                {"onSale", "true"}
            };
            var content2 = new UmbracoContent(item2);

            var item3 = new Dictionary<string, string>
            {
                {"alias", "testItem3"},
                {"itemId", "3"},
                {"price", "30.00"},
                {"onSale", "true"}
            };
            var content3 = new UmbracoContent(item3);

            var mapItem = mapRegistry.For<TestItem>();
            var mapEntity = mapRegistry.For<TestEntity>();
            var queryItem = Substitute.For<ICmsQuery>();
            var queryEntity = Substitute.For<ICmsQuery>();
            queryFactory.Create(mapItem).Returns(queryItem);
            queryFactory.Create(mapEntity).Returns(queryEntity);

            var entityContents = new IContent[] { content };
            var itemContents = new IContent[] { content1, content2, content3 };

            cmsFetcher.Fetch(queryItem).Returns(itemContents);
            cmsFetcher.Fetch(queryEntity).Returns(entityContents);

            var pool = new DataPool(mapRegistry, mapper, cacheRegistry, cacheFetcher, queryFactory, cmsFetcher);

            var entities = await pool.GetAsync<TestEntity>();
            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.List, Is.Not.Null);
            Assert.That(entity.List.Count, Is.EqualTo(2));
            Assert.That(entity.Dictionary, Is.Not.Null);
            Assert.That(entity.Dictionary.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAsync_concurrent_access_success()
        {
            var root = Substitute.For<IRoot>();
            var mapRegistry = new MapRegistry(root);
            var mapper = new Mapper(mapRegistry);
            var cacheRegistry = new CacheRegistry();
            var cacheStore = new CacheStore();
            var cacheFetcher = new CacheFetcher(cacheStore, cacheRegistry);
            var queryFactory = Substitute.For<IQueryFactory>();
            var cmsFetcher = Substitute.For<ICmsFetcher>();

            mapRegistry.Copy<CmsKey>()
                .Map(x => x.Key, "id");
            mapRegistry.Copy<Model>()
                .Map(x => x.Name, "alias");

            mapRegistry.Register<TestItem>()
                .Map(x => x.Key, "itemId");
            mapRegistry.Register<TestEntity>()
                .Fit(x => x.Item)
                .Fit<TestItem>((x, y) =>
                {
                    x.List = y.ToList();
                    x.Dictionary = x.List.ToDictionary(z => z.Key, z => z);
                }, "items", x => x.ItemId.ToString());

            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"item", "2"},
                {"list", "a,b,c"},
                {"items", "1,3"},
            };
            var content = new UmbracoContent(fields);

            var item1 = new Dictionary<string, string>
            {
                {"alias", "testItem1"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"}
            };
            var content1 = new UmbracoContent(item1);

            var item2 = new Dictionary<string, string>
            {
                {"alias", "testItem2"},
                {"itemId", "2"},
                {"price", "20.00"},
                {"onSale", "true"}
            };
            var content2 = new UmbracoContent(item2);

            var item3 = new Dictionary<string, string>
            {
                {"alias", "testItem3"},
                {"itemId", "3"},
                {"price", "30.00"},
                {"onSale", "true"}
            };
            var content3 = new UmbracoContent(item3);

            var mapItem = mapRegistry.For<TestItem>();
            var mapEntity = mapRegistry.For<TestEntity>();
            var queryItem = Substitute.For<ICmsQuery>();
            var queryEntity = Substitute.For<ICmsQuery>();
            queryFactory.Create(mapItem).Returns(queryItem);
            queryFactory.Create(mapEntity).Returns(queryEntity);

            var entityContents = new IContent[] { content };
            var itemContents = new IContent[] { content1, content2, content3 };

            cmsFetcher.Fetch(queryItem).Returns(itemContents);
            cmsFetcher.Fetch(queryEntity).Returns(entityContents);

            var pool = new DataPool(mapRegistry, mapper, cacheRegistry, cacheFetcher, queryFactory, cmsFetcher);

            var tasks = new Task<IEnumerable<TestEntity>>[50];
            for(var i = 0; i < tasks.Length; i++)
                tasks[i] = pool.GetAsync<TestEntity>();

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                var entities = task.Result;

                Assert.That(entities, Is.Not.Null);
                Assert.That(entities.Count(), Is.EqualTo(1));
                var entity = entities.First();
                Assert.That(entity.Item, Is.Not.Null);
                Assert.That(entity.Item.ItemId, Is.EqualTo(2));
                Assert.That(entity.Items, Is.Not.Null);
                Assert.That(entity.Items.Count, Is.EqualTo(2));
            }


        }

    }
}
