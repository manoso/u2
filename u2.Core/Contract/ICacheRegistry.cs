using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ICacheRegistry
    {
        void Add<T>(string key, Func<Task<IEnumerable<T>>> func, int cacheInMins);
        void Add<T>(Func<Task<IEnumerable<T>>> func, int cacheInMins = 0, params ILookupParameter<T>[] lookups);
        bool TryGetTask(string taskKey, out ICacheTask task);
        Task Reload<T>(string key = null);
        Task Reload();
    }
}