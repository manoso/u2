﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace u2.Cache
{
    public interface ICacheRegistry
    {
        void Add<T>(string key, Func<Task<IEnumerable<T>>> func, int cacheInMins);
        void Add<T>(Func<Task<IEnumerable<T>>> func, int cacheInMins = 0, params LookupParameter<T>[] lookups);
        Task<IEnumerable<T>> FetchAsync<T>();
        Task<T> FetchAsync<T>(string key);
        Task<ILookup<string, T>> FetchLookupAsync<T>(LookupParameter<T> lookupParameter);
        Task Reload<T>(string key = null);
        Task Reload();
    }

    public class CacheRegistry : ICacheRegistry
    {
        private readonly ISiteStore _store;
        private readonly IDictionary<string, CacheTask> _tasks = new Dictionary<string, CacheTask>();

        public CacheRegistry(ISiteStore store)
        {
            _store = store;
        }

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

        public void Add<T>(Func<Task<IEnumerable<T>>> func, int cacheInSecs, params LookupParameter<T>[] lookups)
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


        public async Task Reload<T>(string key = null)
        {
            if (_tasks.TryGetValue(key ?? typeof(T).FullName, out CacheTask task))
                await task.Reload();
        }

        public async Task Reload()
        {
            foreach (var task in _tasks.Values)
                await task.Reload();
        }

        public async Task<IEnumerable<T>> FetchAsync<T>()
        {
            return await FetchAsync<IEnumerable<T>>(typeof(T).FullName);
        }

        public async Task<T> FetchAsync<T>(string key)
        {
            return await FetchAsync<T, T>(key);
        }

        public async Task<ILookup<string, T>> FetchLookupAsync<T>(LookupParameter<T> lookupParameter)
        {
            if (lookupParameter == null)
                return null;

            return await FetchAsync<ILookup<string, T>, T>(lookupParameter.GetKey(), true);
        }

        private async Task<TResult> FetchAsync<TResult, T>(string key, bool isLookup = false)
        {
            var type = typeof(T);
            var taskKey = isLookup ? type.FullName : key;

            return _tasks.TryGetValue(taskKey, out CacheTask task)
                ? await TaskFetch<TResult>(task, key)
                : default(TResult);
        }

        private async Task<T> TaskFetch<T>(CacheTask task, string cacheKey)
        {
            if (!_store.Has(cacheKey) || task.IsExpired)
                await task.Run<T>();

            return (T)_store.Get(cacheKey);
        }
    }
}



