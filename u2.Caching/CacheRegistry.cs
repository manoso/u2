using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Caching
{
    public class CacheRegistry : ICacheRegistry
    {
        private readonly IDictionary<string, ICacheTask> _tasks = new Dictionary<string, ICacheTask>();

        public ICacheTask<T> Add<T>(Func<Task<IEnumerable<T>>> func, string key = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                key = typeof(T).FullName;

            var task = new CacheTask<T>
            {
                TaskKey = key,
                Task = func
            };

            _tasks.Add(key, task);

            return task;
        }

        public bool Has<T>()
        {
            return Has(typeof(T).FullName);
        }

        public bool Has(string key)
        {
            return _tasks.Keys.Contains(key);
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





