using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u2.Core;
using u2.Core.Contract;

namespace u2.Caching
{
    public class CacheTask<T> : CacheTask, ICacheTask<T>
    {
        public IList<ICacheLookup<T>> CacheLookups { get; } = new List<ICacheLookup<T>>();

        private readonly Func<ICache, Task<IEnumerable<T>>> _task;

        public ICacheTask<T> Span(int seconds)
        {
            if (seconds < 0)
                seconds = 0;
            CacheInSeconds = seconds;
            return this;
        }

        public ICacheTask<T> Lookup(ICacheLookup<T> cacheLookup)
        {
            CacheLookups.Add(cacheLookup);
            return this;
        }

        private Func<IEnumerable<T>, IEnumerable<T>> _onSave;

        public CacheTask(Func<ICache, Task<IEnumerable<T>>> task, string key = null)
        {
            _task = task;
            if (string.IsNullOrWhiteSpace(key))
                key = typeof(T).FullName;
            TaskKey = key;
        }

        public ICacheTask<T> OnSave(Func<IEnumerable<T>, IEnumerable<T>> func)
        {
            _onSave = func;
            return this;
        }

        protected override Func<ICache, bool> CanRun => IsExpired;

        protected override Action<ICache> Reset
        {
            get
            {
                return x =>
                {
                    var info = GetInfo(x);
                    if (CacheInSeconds > 0)
                        info.Timestamp = DateTime.UtcNow;
                };
            }
        }

        protected override Func<ICache, Task<IDictionary<string, object>>> RunTask => Load;

        protected override async Task<IDictionary<string, object>> Load(ICache cache)
        {
            IList<T> items = null;
            var result = new Dictionary<string, object>();

            if (_task != null)
            {
                var data = await _task(cache).ConfigureAwait(false);

                if (data != null)
                {
                    data = _onSave?.Invoke(data) ?? data;

                    items = data.ToList();
                    result[TaskKey] = items;
                }
            }

            if (CacheLookups != null && CacheLookups.Any() && items != null && items.Any())
            {
                foreach (var lookup in CacheLookups)
                {
                    var data = items.ToLookup(lookup.GetLookupKey);
                    result[lookup.CacheKey] = data;
                }
            }

            return result;
        }
    }

    public abstract class CacheTask : RunOnce<ICache>, ICacheTask
    {
        public int CacheInSeconds { get; set; } = 300;

        public string TaskKey { get; protected set; }

        public bool IsExpired(ICache cache)
        {
            var info = GetInfo(cache);
            return CacheInSeconds <= 0 || info.Timestamp.AddSeconds(CacheInSeconds) <= DateTime.UtcNow;
        }

        private readonly IDictionary<ICache, TaskInfo> _taskInfos = new Dictionary<ICache, TaskInfo>();

        protected override Task<bool> GetTask(ICache cache)
        {
            return GetInfo(cache).Task;
        }

        protected override void SetTask(ICache cache, Task<bool> task)
        {
            var info = GetInfo(cache);
            info.Task = task;
            info.Timestamp = DateTime.MinValue;
        }

        protected TaskInfo GetInfo(ICache cache)
        {
            if (!_taskInfos.TryGetValue(cache, out var info))
            {
                info = _taskInfos[cache] = new TaskInfo();
            }
            return info;
        }

        public async Task Reload(ICache cache)
        {
            var info = GetInfo(cache);
            if (CacheInSeconds > 0)
                info.Timestamp = DateTime.UtcNow.AddSeconds(-CacheInSeconds);
            await Run(cache).ConfigureAwait(false);
        }

        public async Task Run(ICache cache, Action<string, object> save = null)
        {
            var info = GetInfo(cache);
            if (info.Save == null && save != null)
                info.Save = save;

            var done = info.Save == null
                ? null as Action<IDictionary<string, object>>
                : data =>
                {
                    foreach (var cacheItem in data)
                        info.Save(cacheItem.Key, cacheItem.Value);
                };
            await RunAsync(cache, done).ConfigureAwait(false);
        }

        protected abstract Task<IDictionary<string, object>> Load(ICache cache);
    }
}
