
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using u2.Test;

namespace u2.Cache.Test
{
    [TestFixture]
    public class CacheTaskTest
    {
        [Test]
        public async Task Run_concurrent_run_single_result_success()
        {
            Func<Task<IEnumerable<CacheItem>>> func = async () => await Task.Run(() => CacheItems);
            const string key = "CacheItem";
            const int cacheTime = 300;

            var task = new CacheTask<CacheItem>
            {
                TaskKey = key,
                Task = func,
                CacheInSecs = cacheTime
            };

            var tasks = new Task<CacheItem>[50];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = GetFirstAsync(task);
            }

            await Task.WhenAll(tasks);

            var result = tasks[0].Result;
            for (var i = 1; i < tasks.Length; i++)
            {
                Assert.That(result.Id, Is.EqualTo(tasks[i].Result.Id));
            }
        }

        [Test]
        public async Task Run_with_lookups_concurrent_run_single_result_success()
        {
            Func<Task<IEnumerable<CacheItem>>> func = async () => await Task.Run(() => CacheItems);
            const int cacheTime = 300;
            var taskKey = typeof(CacheItem).FullName;
            var loopupParam = new LookupParameter<CacheItem>().Add(x => x.LookupKey);

            var task = new CacheTask<CacheItem>
            {
                TaskKey = taskKey,
                Task = func,
                CacheInSecs = cacheTime,
                LookupParameters = new [] { loopupParam }
            };

            await task.Run();
            var items = task.CacheItems;
            var lookups = items["Lookup_CacheItem_LookupKey"] as ILookup<string, CacheItem>;
            var all = items[taskKey] as IEnumerable<CacheItem>;

            Assert.That(items.Count, Is.EqualTo(2));
            Assert.That(all.Count(), Is.EqualTo(3));
            Assert.That(lookups.Count(), Is.EqualTo(2));

            var lookupKey1 = loopupParam.GetLookupKey(new CacheItem { LookupKey = 1 });
            var lookupKey2 = loopupParam.GetLookupKey(new CacheItem { LookupKey = 2 });

            Assert.That(lookups[lookupKey1].Count(), Is.EqualTo(2));
            Assert.That(lookups[lookupKey2].Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Reload_success()
        {
            Func<Task<IEnumerable<CacheItem>>> func = async () => await Task.Run(() => CacheItems);
            const int cacheTime = 300;
            var taskKey = typeof(CacheItem).FullName;

            var task = new CacheTask<CacheItem>
            {
                TaskKey = taskKey,
                Task = func,
                CacheInSecs = cacheTime,
            };

            await task.Run();
            var before = GetFirst(task);
            await task.Reload();
            var after = GetFirst(task);

            Assert.That(before.Id, Is.Not.EqualTo(after.Id));
        }


        private async Task<T> GetFirstAsync<T>(CacheTask<T> task) where T: class
        {
            await task.Run();
            return ((IEnumerable<T>)task.CacheItems.First().Value).First();
        }
        private T GetFirst<T>(CacheTask<T> task) where T : class
        {
            return ((IEnumerable<T>)task.CacheItems.First().Value).First();
        }

        public IEnumerable<CacheItem> CacheItems => new[]
        {
            new CacheItem { LookupKey = 1},
            new CacheItem { LookupKey = 1},
            new CacheItem { LookupKey = 2}
        };
    }
}
