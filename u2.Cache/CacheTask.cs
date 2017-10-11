﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u2.Core;
using u2.Core.Contract;

namespace u2.Cache
{
    public class CacheTask<T> : CacheTask, ICacheTask<T>
    {
        public ILookupParameter<T>[] LookupParameters { get; set; }

        protected override Func<bool> NeedRun
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

        protected override Task RunAsync => Load();

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

    public abstract class CacheTask : OnceAsync, ICacheTask
    {
        private int _cacheInSecs = 300;
        public int CacheInSecs
        {
            get => _cacheInSecs;
            set
            {
                if (value > 0)
                    _cacheInSecs = value;
            }
        }
        public string TaskKey { get; set; }
        public Delegate Task { get; set; }

        public IDictionary<string, object> CacheItems { get; } = new Dictionary<string, object>();

        public bool IsExpired => CacheInSecs <= 0 || Timestamp.AddSeconds(CacheInSecs) <= DateTime.UtcNow;

        protected DateTime Timestamp;

        /// <summary>
        /// Updates cache tasks' timestamp to be expired. Next subsequent request will be re-evaluated and data refreshed
        /// </summary>
        public async Task Reload()
        {
            Timestamp = DateTime.UtcNow.AddSeconds(-CacheInSecs);
            await Load();
        }

        public async Task Run(Action<string, object> save)
        {
            await Run(() =>
            {
                foreach (var cacheItem in CacheItems)
                    save(cacheItem.Key, cacheItem.Value);
            });
        }

        protected abstract Task Load();
    }
}
