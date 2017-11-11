using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ICacheRegistry
    {
        ICacheTask<T> Add<T>(Func<ICache, Task<IEnumerable<T>>> func, string key = null);
        bool Has<T>();
        bool Has(string key);
        bool TryGetTask(string taskKey, out ICacheTask task);
        Task ReloadAsync<T>(ICache cache, string key = null);
        Task ReloadAsync(ICache cache);
        void Reload<T>(ICache cache, string key = null);
        void Reload(ICache cache);
    }
}