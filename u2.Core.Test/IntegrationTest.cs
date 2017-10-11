using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using u2.Caching;
using u2.Core.Contract;
using u2.Test;
using u2.Umbraco;

namespace u2.Core.Test
{
    [TestFixture]
    public class IntegrationTest
    {
        private ICache Setup(Action<ITypeMap<TestEntity>> fit)
        {
            var root = Substitute.For<IRoot>();
            var mapRegistry = new MapRegistry(root);
            var mapper = new Mapper(mapRegistry);
            var cacheRegistry = new CacheRegistry();
            var cacheStore = new CacheStore();
            var cache = new Cache(cacheStore, cacheRegistry);
            var queryFactory = Substitute.For<IQueryFactory>();
            var cmsFetcher = Substitute.For<ICmsFetcher>();
            var registry = new Registry(mapRegistry, mapper, cacheRegistry, cache, queryFactory, cmsFetcher);

            mapRegistry.Copy<CmsKey>()
                .Map(x => x.Key, "id");
            mapRegistry.Copy<Model>()
                .Map(x => x.Name, "alias");

            registry.Register<TestInfo>()
                .Map(x => x.Key, "infoId");

            registry.Register<TestItem>()
                .Map(x => x.Key, "itemId")
                .Fit(x => x.Infos);

            fit(registry.Register<TestEntity>());

            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"list", "a,b,c"},
                {"item", "2"},
                {"items", "1,3"}
            };
            var content = new UmbracoContent(fields);

            var item1 = new Dictionary<string, string>
            {
                {"alias", "testItem1"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"},
                {"infos", "1,2" }
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

            var info1 = new Dictionary<string, string>
            {
                {"alias", "testInfo1"},
                {"infoId", "1"},
                {"info", "info1" }
            };
            var contentInfo1 = new UmbracoContent(info1);

            var info2 = new Dictionary<string, string>
            {
                {"alias", "testInfo2"},
                {"infoId", "2"},
                {"info", "info2" }
            };
            var contentInfo2 = new UmbracoContent(info2);

            var mapItem = mapRegistry.For<TestItem>() as TypeMap<TestItem>;
            var mapEntity = mapRegistry.For<TestEntity>() as TypeMap<TestEntity>;
            var mapInfo = mapRegistry.For<TestInfo>() as TypeMap<TestInfo>;
            var queryItem = Substitute.For<ICmsQuery<TestItem>>();
            var queryEntity = Substitute.For<ICmsQuery<TestEntity>>();
            var queryInfo = Substitute.For<ICmsQuery<TestInfo>>();

            queryFactory.Create(mapItem).Returns(queryItem);
            queryFactory.Create(mapEntity).Returns(queryEntity);
            queryFactory.Create(mapInfo).Returns(queryInfo);

            var entityContents = new IContent[] { content };
            var itemContents = new IContent[] { content1, content2, content3 };
            var infoContents = new IContent[] { contentInfo2, contentInfo1 };

            cmsFetcher.Fetch(queryItem).Returns(itemContents);
            cmsFetcher.Fetch(queryEntity).Returns(entityContents);
            cmsFetcher.Fetch(queryInfo).Returns(infoContents);

            return cache;
        }

        [Test]
        public async Task FetchAsync_fit_single_success()
        {
            var cache = Setup(map => map.Fit(x => x.Item));
            var entities = await cache.FetchAsync<TestEntity>();

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Item, Is.Not.Null);
            Assert.That(entity.Item.ItemId, Is.EqualTo(2));
        }

        [Test]
        public async Task FetchAsync_fit_single_with_key_success()
        {

            var cache = Setup(map => map.Fit(x => x.Item, x => x.ItemId.ToString()));
            var entities = await cache.FetchAsync<TestEntity>();

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Item, Is.Not.Null);
            Assert.That(entity.Item.ItemId, Is.EqualTo(2));
        }

        [Test]
        public async Task FetchAsync_fit_many_success()
        {
            var cache = Setup(map => map.Fit(x => x.Items));
            var entities = await cache.FetchAsync<TestEntity>();

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Items, Is.Not.Null);
            Assert.That(entity.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task FetchAsync_fit_many_with_key_success()
        {
            var cache = Setup(map => map.Fit(x => x.Items, x => x.ItemId.ToString()));
            var entities = await cache.FetchAsync<TestEntity>();

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Items, Is.Not.Null);
            Assert.That(entity.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetchAsync_fit_action_success()
        {
            var cache = Setup(map => map.Fit<TestItem>((x, y) =>
            {
                x.List = y.ToList();
                x.Dictionary = x.List.ToDictionary(z => z.Key, z => z);
            }, "items"));
            var entities = await cache.FetchAsync<TestEntity>();

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.List, Is.Not.Null);
            Assert.That(entity.List.Count, Is.EqualTo(2));
            Assert.That(entity.Dictionary, Is.Not.Null);
            Assert.That(entity.Dictionary.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task FetchAsync_fit_action_with_key_success()
        {
            var cache = Setup(map => map.Fit<TestItem>((x, y) =>
            {
                x.List = y.ToList();
                x.Dictionary = x.List.ToDictionary(z => z.Key, z => z);
            }, "items", x => x.ItemId.ToString()));
            var entities = await cache.FetchAsync<TestEntity>();

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.List, Is.Not.Null);
            Assert.That(entity.List.Count, Is.EqualTo(2));
            Assert.That(entity.Dictionary, Is.Not.Null);
            Assert.That(entity.Dictionary.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task FetchAsync_concurrent_access_success()
        {
            var cache = Setup(map => map.Fit(x => x.Item)
                .Fit<TestItem>((x, y) =>
                {
                    x.List = y.ToList();
                    x.Items = x.List;
                    x.Dictionary = x.List.ToDictionary(z => z.Key, z => z);
                }, "items", x => x.ItemId.ToString()));

            var tasks = new Task<IEnumerable<TestEntity>>[50];
            for(var i = 0; i < tasks.Length; i++)
                tasks[i] = cache.FetchAsync<TestEntity>();

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
                Assert.That(entity.List, Is.Not.Null);
                Assert.That(entity.List.Count, Is.EqualTo(2));
                Assert.That(entity.Dictionary, Is.Not.Null);
                Assert.That(entity.Dictionary.Count, Is.EqualTo(2));
                var item = entity.Items.First();
                Assert.That(item, Is.Not.Null);
                Assert.That(item.Infos, Is.Not.Null);
                Assert.That(item.Infos.Count(), Is.EqualTo(2));
            }
        }
    }
}
