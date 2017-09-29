using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Cache
{
    public class CacheRegistry : ICacheRegistry
    {
        private readonly IDictionary<string, ICacheTask> _tasks = new Dictionary<string, ICacheTask>();

        public void Add<T>(string key, Func<Task<IEnumerable<T>>> func, int cacheInSecs)
        {
            _tasks.Add(key,
                new CacheTask<T>
                {
                    TaskKey = key,
                    Task = func,
                    CacheInSecs = cacheInSecs
                });
        }

        public void Add<T>(Func<Task<IEnumerable<T>>> func, int cacheInSecs, params ILookupParameter<T>[] lookups)
        {
            var taskKey = typeof(T).FullName;
            _tasks.Add(taskKey,
                new CacheTask<T>
                {
                    TaskKey = taskKey,
                    Task = func,
                    CacheInSecs = cacheInSecs,
                    LookupParameters = lookups
                });
        }

        public bool TryGetTask(string taskKey, out ICacheTask task)
        {
            return _tasks.TryGetValue(taskKey, out task);
        }

        public async Task Reload<T>(string key = null)
        {
            if (_tasks.TryGetValue(key ?? typeof(T).FullName, out ICacheTask task))
                await task.Reload();
        }

        public async Task Reload()
        {
            foreach (var task in _tasks.Values)
                await task.Reload();
        }
    }
}





