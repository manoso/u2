using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace u2.Cache
{
    internal class CacheTask<T> : CacheTask
    {
        internal LookupParameter<T>[] LookupParameters { get; set; }

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private Task<bool> _task;

        internal override async Task Run<TResult>()
        {
            TaskCompletionSource<bool> taskCompletion = null;
            await _semaphore.WaitAsync();
            if (IsExpired)
            {
                taskCompletion = new TaskCompletionSource<bool>();
                _task = taskCompletion.Task;
                if (CacheInMins > NoCache)
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
            var result = await LoadTask(TaskKey, Task as Func<Task<IEnumerable<T>>>);
            var items = result?.ToList();

            if (LookupParameters != null && LookupParameters.Any() && items != null && items.Any())
            {
                foreach (var lookup in LookupParameters)
                {
                    var data = items.ToLookup(lookup.GetLookupKey);
                    CacheItems.Add(lookup.GetKey(), data);
                }
            }
        }
    }

    public abstract class CacheTask
    {
        internal int CacheInMins { get; set; }
        internal string TaskKey { get; set; }
        internal Delegate Task { get; set; }

        internal IDictionary<string, object> CacheItems = new Dictionary<string, object>();

        internal bool IsExpired => CacheInMins <= NoCache || Timestamp.AddMinutes(CacheInMins) <= DateTime.UtcNow;

        protected const int NoCache = 0;
        protected DateTime Timestamp;

        /// <summary>
        /// Updates cache tasks' timestamp to be expired. Next subsequent request will be re-evaluated and data refreshed
        /// </summary>
        internal async Task Reload()
        {
            Timestamp = DateTime.UtcNow.AddMinutes(-CacheInMins);
            await Load();
        }

        internal abstract Task Run<TResult>();

        protected async Task<T> LoadTask<T>(string key, Func<Task<T>> func) where T : class
        {

            if (func != null)
            {
                var data = await func();

                if (data != null)
                {
                    CacheItems.Add(key, data);
                }
                return data;
            }
            return null;
        }

        protected abstract Task Load();
    }
}
