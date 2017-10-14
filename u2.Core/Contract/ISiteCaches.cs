using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ISiteCaches
    {
        void Register<T>(Func<Task<IEnumerable<T>>> func, int cacheInMins = 0);
        void Register<T>(string key, Func<Task<IEnumerable<T>>> func, int cacheInMins = 0);
        void RegisterLookup<T>(Func<Task<IEnumerable<T>>> func, int cacheInMins = 0, params ICacheLookup<T>[] sets);
        Task Refresh(string site = null);
    }
}