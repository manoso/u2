using System.Collections.Generic;
using System.Web;
using NSubstitute;
using NUnit.Framework;
using u2.Caching;
using u2.Core.Contract;
using u2.Fixture;
using u2.Test;

namespace u2.Config.Test
{
    [TestFixture]
    public class ContextTest
    {
        [Test]
        public void Get_singleton_success()
        {
            Context.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());

            var first = Context.MapRegistry;
            var second = Context.MapRegistry;

            Assert.AreEqual(first, second);
        }

        [Test]
        public void Get_root_function_same_success()
        {
            //var cacheRegistry = Substitute.For<ICacheRegistry>();
            //var root1 = new TestRoot { Id = 1, Hosts = new List<string> { "root1" } };
            //var root2 = new TestRoot { Id = 2, Hosts = new List<string> { "root2" } };
            //var defaultCache = SiteCaches.GetDefaultCache(cacheRegistry);

            //SiteCaches.Add(root1, new Cache(new CacheStore(), cacheRegistry));
            //SiteCaches.Add(root2, new Cache(new CacheStore(), cacheRegistry));

            //Resolver.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());

            //HttpContext.Current = new HttpContext(
            //    new HttpRequest("", "http://root2", ""),
            //    new HttpResponse(null)
            //);
            //var first = Resolver.Get<IRoot>();
            //var second = Resolver.Get<IRoot>();

            //Assert.AreEqual(first, second);
            //Assert.That(first.Id, Is.EqualTo(2));
        }

        //[Test]
        //public void Get_root_function_different_success()
        //{
        //    var defaultCache = Substitute.For<ICache>();
        //    defaultCache.Fetch<TestRoot>().Returns(new List<TestRoot>
        //    {
        //        new TestRoot {Id = 1, Hosts = new List<string> {"hoyts1"}},
        //        new TestRoot {Id = 2, Hosts = new List<string> {"hoyts2"}}
        //    });
        //    SiteCaches._default = defaultCache;
        //    Resolver.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());

        //    HttpContext.Current = new HttpContext(
        //        new HttpRequest("", "http://hoyts1", ""),
        //        new HttpResponse(null)
        //    );
        //    var first = Resolver.Get<IRoot>();

        //    HttpContext.Current = new HttpContext(
        //        new HttpRequest("", "http://hoyts2", ""),
        //        new HttpResponse(null)
        //    );
        //    var second = Resolver.Get<IRoot>();

        //    Assert.AreNotEqual(first, second);
        //    Assert.That(first.Id, Is.EqualTo(1));
        //    Assert.That(second.Id, Is.EqualTo(2));
        //}

        //[Test]
        //public void Get_cache_function_same_success()
        //{
        //    var defaultCache = Substitute.For<ICache>();
        //    var root1 = new TestRoot { Id = 1, Hosts = new List<string> { "hoyts1" } };
        //    var root2 = new TestRoot { Id = 2, Hosts = new List<string> { "hoyts2" } };

        //    defaultCache.Fetch<TestRoot>().Returns(new List<TestRoot>
        //    {
        //        root1, root2
        //    });

        //    SiteCaches._default = defaultCache;
        //    Resolver.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());

        //    HttpContext.Current = new HttpContext(
        //        new HttpRequest("", "http://hoyts2", ""),
        //        new HttpResponse(null)
        //    );
        //    var first = Resolver.Get<ICache>();
        //    var second = Resolver.Get<ICache>();

        //    Assert.IsFalse(SiteCaches.Has(root1));
        //    Assert.IsTrue(SiteCaches.Has(root2));
        //    Assert.That(SiteCaches.Get(root2), Is.EqualTo(first));
        //    Assert.AreEqual(first, second);
        //}

        //[Test]
        //public void Get_cache_function_different_success()
        //{
        //    var defaultCache = Substitute.For<ICache>();
        //    var root1 = new TestRoot { Id = 1, Hosts = new List<string> { "hoyts1" } };
        //    var root2 = new TestRoot { Id = 2, Hosts = new List<string> { "hoyts2" } };

        //    defaultCache.Fetch<TestRoot>().Returns(new List<TestRoot>
        //    {
        //        root1, root2
        //    });

        //    SiteCaches._default = defaultCache;
        //    Resolver.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());

        //    HttpContext.Current = new HttpContext(
        //        new HttpRequest("", "http://hoyts1", ""),
        //        new HttpResponse(null)
        //    );
        //    var first = Resolver.Get<ICache>();

        //    HttpContext.Current = new HttpContext(
        //        new HttpRequest("", "http://hoyts2", ""),
        //        new HttpResponse(null)
        //    );
        //    var second = Resolver.Get<ICache>();

        //    Assert.IsTrue(SiteCaches.Has(root1));
        //    Assert.That(SiteCaches.Get(root1), Is.EqualTo(first));
        //    Assert.IsTrue(SiteCaches.Has(root2));
        //    Assert.That(SiteCaches.Get(root2), Is.EqualTo(second));
        //    Assert.AreNotEqual(first, second);
        //}

        //[Test]
        //public void Get_root_function_no_match_return_first_success()
        //{
        //    var defaultCache = Substitute.For<ICache>();
        //    var root1 = new TestRoot { Id = 1, Hosts = new List<string> { "hoyts1" } };
        //    var root2 = new TestRoot { Id = 2, Hosts = new List<string> { "hoyts2" } };

        //    defaultCache.Fetch<TestRoot>().Returns(new List<TestRoot>
        //    {
        //        root1, root2
        //    });

        //    SiteCaches._default = defaultCache;
        //    Resolver.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());

        //    HttpContext.Current = new HttpContext(
        //        new HttpRequest("", "http://hoyts", ""),
        //        new HttpResponse(null)
        //    );
        //    var first = Resolver.Get<IRoot>();
        //    var second = Resolver.Get<IRoot>();

        //    Assert.That(first.Id, Is.EqualTo(1));
        //    Assert.AreEqual(first, second);
        //}
    }
}
