using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ICacheRegistry
    {
        void Add<T>(Func<Task<IEnumerable<T>>> func, int cacheInSecs = 0, string key = null, params ILookupParameter<T>[] lookups);
        bool Has<T>();
        bool Has(string key);
        bool TryGetTask(string taskKey, out ICacheTask task);
        Task Reload<T>(string key = null);
        Task Reload();
    }
}