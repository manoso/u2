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

        private readonly Func<Task<IEnumerable<T>>> _task;

        public ICacheTask<T> Span(int seconds)
        {
            if (seconds > 0)
                CacheInSecs = seconds;
            return this;
        }

        public ICacheTask<T> Lookup(ICacheLookup<T> cacheLookup)
        {
            CacheLookups.Add(cacheLookup);
            return this;
        }

        private Func<IEnumerable<T>, IEnumerable<T>> _onSave;

        public CacheTask(Func<Task<IEnumerable<T>>> task)
        {
            _task = task;
        }

        public ICacheTask<T> OnSave(Func<IEnumerable<T>, IEnumerable<T>> func)
        {
            _onSave = func;
            return this;
        }

        protected override Func<bool> CanRun
        {
            get { return () => IsExpired; }
        } 

        protected override Action Reset
        {
            get
            {
                return () =>
                {
                    if (CacheInSecs > 0)
                        Timestamp = DateTime.UtcNow;
                };
            }
        }

        protected override Task RunTask => Load();

        protected override async Task Load()
        {
            IList<T> items = null;
            if (_task != null)
            {
                var data = await _task().ConfigureAwait(false);

                if (data != null)
                {
                    data = _onSave?.Invoke(data) ?? data;

                    items = data.ToList();
                    CacheItems[TaskKey] = items;
                }
            }

            if (CacheLookups != null && CacheLookups.Any() && items != null && items.Any())
            {
                foreach (var lookup in CacheLookups)
                {
                    var data = items.ToLookup(lookup.GetLookupKey);
                    CacheItems[lookup.CacheKey] = data;
                }
            }
        }
    }

    public abstract class CacheTask : RunOnce, ICacheTask
    {
        protected int CacheInSecs = 300;

        public string TaskKey { get; set; }

        public IDictionary<string, object> CacheItems { get; } = new Dictionary<string, object>();

        public bool IsExpired => CacheInSecs <= 0 || Timestamp.AddSeconds(CacheInSecs) <= DateTime.UtcNow;

        protected DateTime Timestamp;

        /// <summary>
        /// Updates cache tasks' timestamp to be expired. Next subsequent request will be re-evaluated and data refreshed
        /// </summary>
        public async Task Reload()
        {
            Timestamp = DateTime.UtcNow.AddSeconds(-CacheInSecs);
            await Load().ConfigureAwait(false);
        }

        public async Task Run(Action<string, object> save = null)
        {
            var done = save == null
                ? null as Action
                : () =>
                {
                    foreach (var cacheItem in CacheItems)
                        save(cacheItem.Key, cacheItem.Value);
                };
            await RunAsync(done).ConfigureAwait(false);
        }

        protected abstract Task Load();
    }
}
