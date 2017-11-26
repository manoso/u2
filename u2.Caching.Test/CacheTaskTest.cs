using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;

namespace u2.Caching.Test
{
    [TestFixture]
    public class CacheTaskTest
    {
        private readonly Func<ICache, Task<IEnumerable<CacheItem>>> _task = async x => await Task.Run(() => CacheItems).ConfigureAwait(false);

        [Test]
        public async Task Run_concurrent_run_single_result_success()
        {
            const string key = "CacheItem";
            const int cacheTime = 300;

            var task = new CacheTask<CacheItem>(_task)
            {
                TaskKey = key
            }.Span(cacheTime);

            var tasks = new Task<CacheItem>[50];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = GetFirstAsync(task);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            var result = tasks[0].Result;
            for (var i = 1; i < tasks.Length; i++)
            {
                Assert.That(result.Id, Is.EqualTo(tasks[i].Result.Id));
            }
        }

        [Test]
        public async Task Run_with_lookups_concurrent_run_single_result_success()
        {
            const int cacheTime = 300;
            var taskKey = typeof(CacheItem).FullName;
            var lookup = new CacheLookup<CacheItem>().Add(x => x.LookupKey);

            var task = new CacheTask<CacheItem>(_task)
            {
                TaskKey = taskKey
            }.Span(cacheTime);
            task.Lookup(lookup);

            var cache = Substitute.For<ICache>();

            await task.Run(cache).ConfigureAwait(false);
            var items = task.CacheItems;
            var lookups = items["Lookup_CacheItem_LookupKey"] as ILookup<string, CacheItem>;
            var all = items[taskKey] as IEnumerable<CacheItem>;

            Assert.That(items.Count, Is.EqualTo(2));
            Assert.That(all.Count(), Is.EqualTo(3));
            Assert.That(lookups.Count(), Is.EqualTo(2));

            var lookupKey1 = lookup.GetLookupKey(new CacheItem { LookupKey = 1 });
            var lookupKey2 = lookup.GetLookupKey(new CacheItem { LookupKey = 2 });

            Assert.That(lookups[lookupKey1].Count(), Is.EqualTo(2));
            Assert.That(lookups[lookupKey2].Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Reload_success()
        {
            const int cacheTime = 300;
            var taskKey = typeof(CacheItem).FullName;

            var task = new CacheTask<CacheItem>(_task)
            {
                TaskKey = taskKey
            }.Span(cacheTime);

            var cache = Substitute.For<ICache>();

            await task.Run(cache).ConfigureAwait(false);
            var before = GetFirst(task);
            await task.Reload(cache).ConfigureAwait(false);
            var after = GetFirst(task);

            Assert.That(before.Id, Is.Not.EqualTo(after.Id));
        }

        private async Task<T> GetFirstAsync<T>(ICacheTask<T> task) where T: class
        {
            var cache = Substitute.For<ICache>();
            await task.Run(cache).ConfigureAwait(false);
            return ((IEnumerable<T>)task.CacheItems.First().Value).First();
        }

        private T GetFirst<T>(ICacheTask<T> task) where T : class
        {
            return ((IEnumerable<T>)task.CacheItems.First().Value).First();
        }

        private static IEnumerable<CacheItem> CacheItems => new[]
        {
            new CacheItem { LookupKey = 1},
            new CacheItem { LookupKey = 1},
            new CacheItem { LookupKey = 2}
        };
    }
}
