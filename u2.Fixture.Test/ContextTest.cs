using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;
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
            var root1 = new TestRoot
            {
                Id = 1,
                Key = Guid.NewGuid(),
                Hosts = new List<string> { "root1" },
                CacheName = "one"
            };
            var root2 = new TestRoot
            {
                Id = 2,
                Key = Guid.NewGuid(),
                Hosts = new List<string> { "root2" },
                CacheName = "two"
            };

            async Task<IEnumerable<TestRoot>> GetRoots(ICache cache)
            {
                return await Task.Run(() =>
                    new[]
                    {
                        root1, root2
                    }).ConfigureAwait(false);
            }

            Context.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());
            Context.CacheRegistry.Add(GetRoots);

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://root2", ""),
                new HttpResponse(null)
            );
            var first = Context.Root;
            var second = Context.Root;

            Assert.AreEqual(first, second);
            Assert.AreEqual(first.Id, 2);
        }

        [Test]
        public void Get_root_function_different_success()
        {
            var root1 = new TestRoot
            {
                Id = 1,
                Key = Guid.NewGuid(),
                Hosts = new List<string> { "root1" },
                CacheName = "one"
            };
            var root2 = new TestRoot
            {
                Id = 2,
                Key = Guid.NewGuid(),
                Hosts = new List<string> { "root2" },
                CacheName = "two"
            };

            async Task<IEnumerable<TestRoot>> GetRoots(ICache cache)
            {
                return await Task.Run(() =>
                    new[]
                    {
                        root1, root2
                    }).ConfigureAwait(false);
            }

            Context.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());
            Context.CacheRegistry.Add(GetRoots);

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://root1", ""),
                new HttpResponse(null)
            );
            var first = Context.Root;

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://root2", ""),
                new HttpResponse(null)
            );
            var second = Context.Root;

            Assert.AreNotEqual(first, second);
            Assert.AreEqual(first.Id, 1);
            Assert.AreEqual(second.Id, 2);
        }

        [Test]
        public void Get_cache_function_same_success()
        {
            var root1 = new TestRoot
            {
                Id = 1,
                Key = Guid.NewGuid(),
                Hosts = new List<string> { "root1" },
                CacheName = "one"
            };
            var root2 = new TestRoot
            {
                Id = 2,
                Key = Guid.NewGuid(),
                Hosts = new List<string> { "root2" },
                CacheName = "two"
            };

            async Task<IEnumerable<TestRoot>> GetRoots(ICache cache)
            {
                return await Task.Run(() =>
                    new[]
                    {
                        root1, root2
                    }).ConfigureAwait(false);
            }

            Context.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());
            Context.CacheRegistry.Add(GetRoots);

            var cache1 = Context.SiteCaches.Get(root1);
            var cache2 = Context.SiteCaches.Get(root2);

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://root2", ""),
                new HttpResponse(null)
            );
            var first = Context.Cache;
            var second = Context.Cache;

            Assert.AreEqual(first, second);
            Assert.AreEqual(cache2, second);
            Assert.AreNotEqual(first, cache1);
        }

        [Test]
        public void Get_cache_function_different_success()
        {
            var root1 = new TestRoot
            {
                Id = 1,
                Key = Guid.NewGuid(),
                Hosts = new List<string> { "root1" },
                CacheName = "one"
            };
            var root2 = new TestRoot
            {
                Id = 2,
                Key = Guid.NewGuid(),
                Hosts = new List<string> { "root2" },
                CacheName = "two"
            };

            async Task<IEnumerable<TestRoot>> GetRoots(ICache cache)
            {
                return await Task.Run(() =>
                    new[]
                    {
                        root1, root2
                    }).ConfigureAwait(false);
            }

            Context.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());
            Context.CacheRegistry.Add(GetRoots);

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://root1", ""),
                new HttpResponse(null)
            );
            var first = Context.Cache;

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://root2", ""),
                new HttpResponse(null)
            );
            var second = Context.Cache;

            var cache1 = Context.SiteCaches.Get(root1);
            var cache2 = Context.SiteCaches.Get(root2);

            Assert.AreNotEqual(first, second);
            Assert.AreEqual(first, cache1);
            Assert.AreEqual(second, cache2);
        }

        [Test]
        public void Get_root_function_no_match_return_first_success()
        {
            var root1 = new TestRoot
            {
                Id = 1,
                Key = Guid.NewGuid(),
                Hosts = new List<string> { "root1" },
                CacheName = "one"
            };
            var root2 = new TestRoot
            {
                Id = 2,
                Key = Guid.NewGuid(),
                Hosts = new List<string> { "root2" },
                CacheName = "two"
            };

            async Task<IEnumerable<TestRoot>> GetRoots(ICache cache)
            {
                return await Task.Run(() =>
                    new[]
                    {
                        root1, root2
                    }).ConfigureAwait(false);
            }

            Context.Init<TestRoot>(new TestCacheConfig(), new TestUmbracoConfig(), new TestMapBuild(), new TestCacheBuild());
            Context.CacheRegistry.Add(GetRoots);

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://root", ""),
                new HttpResponse(null)
            );
            var first = Context.Root;
            var second = Context.Root;

            Assert.AreEqual(first, second);
            Assert.AreEqual(first.Id, 1);
        }
    }
}
