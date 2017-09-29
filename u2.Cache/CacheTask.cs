using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Cache
{
    public class CacheTask<T> : CacheTask
    {
        public ILookupParameter<T>[] LookupParameters { get; set; }

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private Task<bool> _task;

        public override async Task Run<TResult>()
        {
            TaskCompletionSource<bool> taskCompletion = null;
            await _semaphore.WaitAsync();
            if (IsExpired)
            {
                taskCompletion = new TaskCompletionSource<bool>();
                _task = taskCompletion.Task;
                if (CacheInSecs > 0)
                    Timestamp = DateTime.UtcNow;
            }
            _semaphore.Release();

            if (taskCompletion != null)
            {
                await Load();
                taskCompletion.SetResult(true);
            }

            await _task;
        }

        protected override async Task Load()
        {
            IList<T> items = null;
            if (Task is Func<Task<IEnumerable<T>>> func)
            {
                var data = await func();

                if (data != null)
                {
                    CacheItems[TaskKey] = data;
                    items = data.ToList();
                }
            }

            if (LookupParameters != null && LookupParameters.Any() && items != null && items.Any())
            {
                foreach (var lookup in LookupParameters)
                {
                    var data = items.ToLookup(lookup.GetLookupKey);
                    CacheItems[lookup.CacheKey] = data;
                }
            }
        }
    }

    public abstract class CacheTask : ICacheTask
    {
        public int CacheInSecs { get; set; }
        public string TaskKey { get; set; }
        public Delegate Task { get; set; }

        public IDictionary<string, object> CacheItems { get; } = new Dictionary<string, object>();

        public bool IsExpired => CacheInSecs <= 0 || Timestamp.AddSeconds(CacheInSecs) <= DateTime.UtcNow;

        protected DateTime Timestamp;

        public abstract Task Run<TResult>();

        /// <summary>
        /// Updates cache tasks' timestamp to be expired. Next subsequent request will be re-evaluated and data refreshed
        /// </summary>
        public async Task Reload()
        {
            Timestamp = DateTime.UtcNow.AddSeconds(-CacheInSecs);
            await Load();
        }

        protected abstract Task Load();
    }
}
