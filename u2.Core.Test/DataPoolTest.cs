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
        private IMapRegistry _mapRegistry;
        private ICacheRegistry _cacheRegistry;
        private ICacheFetcher _cacheFetcher;
        private IQueryFactory _queryFactory;
        private ICmsFetcher _cmsFetcher;

        [OneTimeSetUp]
        public void Setup()
        {
            var root = Substitute.For<IRoot>();
            _mapRegistry = new MapRegistry(root);
            _map = new Map(_mapRegistry);
            _cacheRegistry = new CacheRegistry();
            var cacheStore = new CacheStore();
            _cacheFetcher = new CacheFetcher(cacheStore, _cacheRegistry);
            _queryFactory = Substitute.For<IQueryFactory>();
            _cmsFetcher = Substitute.For<ICmsFetcher>();

            _mapRegistry.Copy<CmsKey>()
                .Map(x => x.Key, "id");
            _mapRegistry.Copy<Model>()
                .Map(x => x.Name, "nodeName");

            _mapRegistry.Register<TestItem>();
            _mapRegistry.Register<TestEntity>()
                .Map(x => x.Name, "contentName")
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
            var mapItem = _mapRegistry.For<TestItem>();
            var mapEntity = _mapRegistry.For<TestEntity>();
            var queryItem = Substitute.For<ICmsQuery>();
            var queryEntity = Substitute.For<ICmsQuery>();
            _queryFactory.Create((TypeMap) mapItem).Returns(queryItem);
            _queryFactory.Create((TypeMap) mapEntity).Returns(queryEntity);
            var content = new UmbracoContent(fields);
            var content3 = new UmbracoContent(item3);

            var entityContents = new IContent[] {content};
            var itemContents = new IContent[] { content1, content2, content3 };

            _cmsFetcher.Fetch(queryItem).Returns(itemContents);
            _cmsFetcher.Fetch(queryEntity).Returns(entityContents);

            var pool = new DataPool(_map, _mapRegistry, _cacheRegistry, _cacheFetcher, _queryFactory, _cmsFetcher);

            var entities = await pool.GetAsync<TestEntity>();
            Assert.That(entities, Is.Not.Null);
        }
    }
}
