﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;
using u2.Umbraco;

namespace u2.Core.Test
{
    [TestFixture]
    public class IntegrationTest
    {
        private ICache Setup(Action<IMapTask<TestEntity>> fit)
        {
            var mapRegistry = new MapRegistry();
            var mapper = new Mapper(mapRegistry);
            var root = Substitute.For<ISite>();
            var cacheRegistry = new CacheRegistry(new TestCacheConfig());
            var cacheStore = new CacheStore();
            var cache = new Cache(cacheStore, cacheRegistry, root);
            var queryFactory = Substitute.For<IQueryFactory>();
            var cmsFetcher = Substitute.For<ICmsFetcher>();
            var registry = new Registry(mapRegistry, mapper, cacheRegistry, queryFactory, cmsFetcher);

            mapRegistry.Copy<CmsKey>()
                .Map(x => x.Key, "key");
            mapRegistry.Copy<Model>()
                .Map(x => x.Name, "alias");

            registry.Register<TestInfo>();

            registry.Register<TestItem>()
                .MatchMany(x => x.Infos);

            fit(registry.Register<TestEntity>());

            var guid1 = Guid.NewGuid().ToString("N");
            var guid2 = Guid.NewGuid().ToString("N");
            var guid3 = Guid.NewGuid().ToString("N");
            var guid4 = Guid.NewGuid().ToString("N");
            var guid5 = Guid.NewGuid().ToString("N");
            var guid6 = Guid.NewGuid().ToString("N");

            var entity = new Dictionary<string, string>
            {
                {"key", guid1 },
                {"alias", "test"},
                {"id", "1000"},
                {"list", "a,b,c"},
                {"item", guid3},
                {"items", guid2+","+guid4}
            };
            var content = new UmbracoContent(entity);

            var item1 = new Dictionary<string, string>
            {
                {"key", guid2 },
                {"alias", "testItem1"},
                {"itemId", "1"},
                {"price", "10.00"},
                {"onSale", "true"},
                {"infos", guid5+","+guid6}
            };
            var content1 = new UmbracoContent(item1);

            var item2 = new Dictionary<string, string>
            {
                {"key", guid3 },
                {"alias", "testItem2"},
                {"itemId", "2"},
                {"price", "20.00"},
                {"onSale", "true"}
            };
            var content2 = new UmbracoContent(item2);

            var item3 = new Dictionary<string, string>
            {
                {"key", guid4 },
                {"alias", "testItem3"},
                {"itemId", "3"},
                {"price", "30.00"},
                {"onSale", "true"}
            };
            var content3 = new UmbracoContent(item3);

            var info1 = new Dictionary<string, string>
            {
                {"key", guid5 },
                {"alias", "testInfo1"},
                {"infoId", "1"},
                {"info", "info1"}
            };
            var contentInfo1 = new UmbracoContent(info1);

            var info2 = new Dictionary<string, string>
            {
                {"key", guid6 },
                {"alias", "testInfo2"},
                {"infoId", "2"},
                {"info", "info2"}
            };
            var contentInfo2 = new UmbracoContent(info2);

            var mapItem = mapRegistry.For<TestItem>() as MapTask<TestItem>;
            var mapEntity = mapRegistry.For<TestEntity>() as MapTask<TestEntity>;
            var mapInfo = mapRegistry.For<TestInfo>() as MapTask<TestInfo>;
            var queryItem = Substitute.For<ICmsQuery<TestItem>>();
            var queryEntity = Substitute.For<ICmsQuery<TestEntity>>();
            var queryInfo = Substitute.For<ICmsQuery<TestInfo>>();

            queryFactory.Create(root, mapItem).Returns(queryItem);
            queryFactory.Create(root, mapEntity).Returns(queryEntity);
            queryFactory.Create(root, mapInfo).Returns(queryInfo);

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
            var cache = Setup(map => map.Match(x => x.Item));
            var entities = await cache.FetchAsync<TestEntity>().ConfigureAwait(false);

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Item, Is.Not.Null);
            Assert.That(entity.Item.ItemId, Is.EqualTo(2));
        }

        [Test]
        public void Fetch_fit_single_success()
        {
            var cache = Setup(map => map.Match(x => x.Item));
            var entities = cache.Fetch<TestEntity>();

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Item, Is.Not.Null);
            Assert.That(entity.Item.ItemId, Is.EqualTo(2));
        }

        [Test]
        public async Task FetchAsync_fit_single_with_key_success()
        {

            var cache = Setup(map => map.Match(x => x.Item, (x, key) => x.Key == Guid.Parse(key)));
            var entities = await cache.FetchAsync<TestEntity>().ConfigureAwait(false);

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Item, Is.Not.Null);
            Assert.That(entity.Item.ItemId, Is.EqualTo(2));
        }

        [Test]
        public void Fetch_fit_single_with_key_success()
        {
            var cache = Setup(map => map.Match(x => x.Item, (x, key) => x.Key == Guid.Parse(key)));
            var entities = cache.Fetch<TestEntity>();

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Item, Is.Not.Null);
            Assert.That(entity.Item.ItemId, Is.EqualTo(2));
        }

        [Test]
        public async Task FetchAsync_fit_many_success()
        {
            var cache = Setup(map => map.MatchMany(x => x.Items));
            var entities = await cache.FetchAsync<TestEntity>().ConfigureAwait(false);

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Items, Is.Not.Null);
            Assert.That(entity.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public void Fetch_fit_many_success()
        {
            var cache = Setup(map => map.MatchMany(x => x.Items));
            var entities = cache.Fetch<TestEntity>();

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Items, Is.Not.Null);
            Assert.That(entity.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task FetchAsync_fit_many_with_key_success()
        {
            var cache = Setup(map => map.MatchMany(x => x.Items, (x, key) => x.Key == Guid.Parse(key)));
            var entities = await cache.FetchAsync<TestEntity>().ConfigureAwait(false);

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Items, Is.Not.Null);
            Assert.That(entity.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public void Fetch_fit_many_with_key_success()
        {
            var cache = Setup(map => map.MatchMany(x => x.Items, (x, key) => x.Key == Guid.Parse(key)));
            var entities = cache.Fetch<TestEntity>();

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.Items, Is.Not.Null);
            Assert.That(entity.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetchAsync_fit_action_success()
        {
            var cache = Setup(map => map.MatchAction<TestItem>((x, y) =>
            {
                x.List = y.ToList();
                x.Dictionary = x.List.ToDictionary(z => z.Key.ToString(), z => z);
            }, "items"));
            var entities = await cache.FetchAsync<TestEntity>().ConfigureAwait(false);

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.List, Is.Not.Null);
            Assert.That(entity.List.Count, Is.EqualTo(2));
            Assert.That(entity.Dictionary, Is.Not.Null);
            Assert.That(entity.Dictionary.Count, Is.EqualTo(2));
        }

        [Test]
        public void Fetch_fit_action_success()
        {
            var cache = Setup(map => map.MatchAction<TestItem>((x, y) =>
            {
                x.List = y.ToList();
                x.Dictionary = x.List.ToDictionary(z => z.Key.ToString(), z => z);
            }, "items"));
            var entities = cache.Fetch<TestEntity>();

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
            var cache = Setup(map => map.MatchAction<TestItem>((x, y) =>
            {
                x.List = y.ToList();
                x.Dictionary = x.List.ToDictionary(z => z.Key.ToString(), z => z);
            }, "items", (x, key) => x.Key == Guid.Parse(key)));
            var entities = await cache.FetchAsync<TestEntity>().ConfigureAwait(false);

            Assert.That(entities, Is.Not.Null);
            Assert.That(entities.Count(), Is.EqualTo(1));
            var entity = entities.First();
            Assert.That(entity.List, Is.Not.Null);
            Assert.That(entity.List.Count, Is.EqualTo(2));
            Assert.That(entity.Dictionary, Is.Not.Null);
            Assert.That(entity.Dictionary.Count, Is.EqualTo(2));
        }

        [Test]
        public void Fetch_fit_action_with_key_success()
        {
            var cache = Setup(map => map.MatchAction<TestItem>((x, y) =>
            {
                x.List = y.ToList();
                x.Dictionary = x.List.ToDictionary(z => z.Key.ToString(), z => z);
            }, "items", (x, key) => x.Key == Guid.Parse(key)));
            var entities = cache.Fetch<TestEntity>();

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
            var cache = Setup(map => map.Match(x => x.Item)
                .MatchAction<TestItem>((x, y) =>
                {
                    x.List = y.ToList();
                    x.Items = x.List;
                    x.Dictionary = x.List.ToDictionary(z => z.Key.ToString(), z => z);
                }, "items", (x, key) => x.Key == Guid.Parse(key)));

            var tasks = new Task<IEnumerable<TestEntity>>[50];
            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = cache.FetchAsync<TestEntity>();

            await Task.WhenAll(tasks).ConfigureAwait(false);

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

        [Test]
        public void Fetch_concurrent_access_success()
        {
            var cache = Setup(map => map.Match(x => x.Item)
                .MatchAction<TestItem>((x, y) =>
                {
                    x.List = y.ToList();
                    x.Items = x.List;
                    x.Dictionary = x.List.ToDictionary(z => z.Key.ToString(), z => z);
                }, "items", (x, key) => x.Key == Guid.Parse(key)));

            var arr = new IEnumerable<TestEntity>[50];
            for (var i = 0; i < arr.Length; i++)
            {
                var index = i;
                var thread = new Thread(() => arr[index] = cache.Fetch<TestEntity>());
                thread.Start();
                thread.Join();
            }

            foreach (var entities in arr)
            {
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

        [Test]
        public async Task FetchAsync_lookup_test()
        {
            var root = Substitute.For<ISite>();
            var cacheRegistry = new CacheRegistry(new TestCacheConfig());
            var cacheStore = new CacheStore();
            var lookup = new CacheLookup<CacheItem>().Add(x => x.LookupKey);
            var lookupOther = new CacheLookup<CacheItem>().Add(x => x.LookupKeyOther);
            var cache = new Cache(cacheStore, cacheRegistry, root);

            async Task<IEnumerable<CacheItem>> Task(ICache c) => await System.Threading.Tasks.Task.Run(() => new[]
            {
                new CacheItem {LookupKey = 1, LookupKeyOther = "2"},
                new CacheItem {LookupKey = 1, LookupKeyOther = "2"},
                new CacheItem {LookupKey = 2, LookupKeyOther = "1"}
            }).ConfigureAwait(false);

            cacheRegistry.Add(Task).Lookup(lookup).Lookup(lookupOther).Span(300);

            var result = await cache.FetchAsync(lookup).ConfigureAwait(false);
            var resultOther = await cache.FetchAsync(lookupOther).ConfigureAwait(false);
            var item = result["2"].First();
            var itemOther = resultOther["1"].First();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result["1"].Count(), Is.EqualTo(2));
            Assert.That(result["2"].Count(), Is.EqualTo(1));

            Assert.That(resultOther, Is.Not.Null);
            Assert.That(resultOther.Count, Is.EqualTo(2));
            Assert.That(resultOther["1"].Count(), Is.EqualTo(1));
            Assert.That(resultOther["2"].Count(), Is.EqualTo(2));

            Assert.That(item.Id, Is.EqualTo(itemOther.Id));
        }

        [Test]
        public void Fetch_lookup_test()
        {
            var root = Substitute.For<ISite>();
            var cacheRegistry = new CacheRegistry(new TestCacheConfig());
            var cacheStore = new CacheStore();
            var lookup = new CacheLookup<CacheItem>().Add(x => x.LookupKey);
            var lookupOther = new CacheLookup<CacheItem>().Add(x => x.LookupKeyOther);
            var cache = new Cache(cacheStore, cacheRegistry, root);

            async Task<IEnumerable<CacheItem>> Task(ICache c) => await System.Threading.Tasks.Task.Run(() => new[]
            {
                new CacheItem {LookupKey = 1, LookupKeyOther = "2"},
                new CacheItem {LookupKey = 1, LookupKeyOther = "2"},
                new CacheItem {LookupKey = 2, LookupKeyOther = "1"}
            }).ConfigureAwait(false);

            cacheRegistry.Add(Task).Lookup(lookup).Lookup(lookupOther).Span(300);

            var result = cache.Fetch(lookup);
            var resultOther = cache.Fetch(lookupOther);
            var item = result["2"].First();
            var itemOther = resultOther["1"].First();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result["1"].Count(), Is.EqualTo(2));
            Assert.That(result["2"].Count(), Is.EqualTo(1));

            Assert.That(resultOther, Is.Not.Null);
            Assert.That(resultOther.Count, Is.EqualTo(2));
            Assert.That(resultOther["1"].Count(), Is.EqualTo(1));
            Assert.That(resultOther["2"].Count(), Is.EqualTo(2));

            Assert.That(item.Id, Is.EqualTo(itemOther.Id));
        }

        [Test]
        public async Task FetchAsync_OnSave_test()
        {
            var root = Substitute.For<ISite>();
            var cacheRegistry = new CacheRegistry(new TestCacheConfig());
            var cacheStore = new CacheStore();
            var cache = new Cache(cacheStore, cacheRegistry, root);

            async Task<IEnumerable<TestItem>> Task(ICache c) => await System.Threading.Tasks.Task.Run(() => new[]
            {
                new TestItem {ItemId = 1},
                new TestItem {ItemId = 3},
                new TestItem {ItemId = 2}
            }).ConfigureAwait(false);
            cacheRegistry.Add(Task)
                .Span(300)
                .OnSave(x => x.OrderBy(y => y.ItemId));

            var result = (await cache.FetchAsync<TestItem>().ConfigureAwait(false)).ToList();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.First().ItemId, Is.EqualTo(1));
            Assert.That(result.Last().ItemId, Is.EqualTo(3));
        }

        [Test]
        public void Fetch_OnSave_test()
        {
            var root = Substitute.For<ISite>();
            var cacheRegistry = new CacheRegistry(new TestCacheConfig());
            var cacheStore = new CacheStore();
            var cache = new Cache(cacheStore, cacheRegistry, root);

            async Task<IEnumerable<TestItem>> Task(ICache c) => await System.Threading.Tasks.Task.Run(() => new[]
            {
                new TestItem {ItemId = 1},
                new TestItem {ItemId = 3},
                new TestItem {ItemId = 2}
            }).ConfigureAwait(false);
            cacheRegistry.Add(Task)
                .Span(300)
                .OnSave(x => x.OrderBy(y => y.ItemId));

            var result = cache.Fetch<TestItem>().ToList();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.First().ItemId, Is.EqualTo(1));
            Assert.That(result.Last().ItemId, Is.EqualTo(3));
        }
    }
}
