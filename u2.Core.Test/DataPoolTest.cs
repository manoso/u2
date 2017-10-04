using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using u2.Cache;
using u2.Core.Contract;
using u2.Umbraco;

namespace u2.Core.Test
{
    [TestFixture]
    public class DataPoolTest
    {
        private IMap _map;
        private ICache _cache;
        private IQueryFactory _queryFactory;
        private ICmsFetcher _cmsFetcher;

        [OneTimeSetUp]
        public void Setup()
        {
            var root = Substitute.For<IRoot>();
            var mapRegistry = new MapRegistry(root);
            var mapper = new Mapper(mapRegistry);
            _map = new Map(mapRegistry, mapper);
            var cacheRegistry = new CacheRegistry();
            var cacheStore = new CacheStore();
            var cacheFetcher = new CacheFetcher(cacheStore, cacheRegistry);
            _cache = new Cache.Cache(cacheRegistry, cacheFetcher);
            _queryFactory = Substitute.For<IQueryFactory>();
            _cmsFetcher = Substitute.For<ICmsFetcher>();

            mapRegistry.Copy<CmsKey>()
                .Map(x => x.Key, "id");
            mapRegistry.Copy<Model>()
                .Map(x => x.Name, "alias");

            mapRegistry.Register<TestItem>()
                .Map(x => x.Key, "itemId");
            mapRegistry.Register<TestEntity>()
                .Map(x => x.Infos, "list", x => x.Split<string>(new[] { ',' }))
                .Tie(x => x.Items);
        }

        [Test]
        public async Task FetchAsync_success()
        {
            var fields = new Dictionary<string, string>
            {
                {"alias", "test"},
                {"id", "1000"},
                {"list", "a,b,c"},
                {"items", "1,3"}
            };
            var content = new UmbracoContent(fields);

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

            var mapItem = _map.For<TestItem>();
            var mapEntity = _map.For<TestEntity>();
            var queryItem = Substitute.For<ICmsQuery>();
            var queryEntity = Substitute.For<ICmsQuery>();
            _queryFactory.Create(mapItem).Returns(queryItem);
            _queryFactory.Create(mapEntity).Returns(queryEntity);

            var entityContents = new IContent[] {content};
            var itemContents = new IContent[] { content1, content2, content3 };

            _cmsFetcher.Fetch(queryItem).Returns(itemContents);
            _cmsFetcher.Fetch(queryEntity).Returns(entityContents);

            var pool = new DataPool(_map, _cache, _queryFactory, _cmsFetcher);

            var entities = await pool.GetAsync<TestEntity>();
            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Items, Is.Not.Null);
            Assert.That(entity.Items.Count, Is.EqualTo(2));
        }
    }
}
