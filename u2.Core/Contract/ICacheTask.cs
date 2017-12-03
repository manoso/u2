using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ICacheTask
    {
        string TaskKey { get; }

        bool IsExpired { get; }

        /// <summary>
        /// Expired the cache tasks' timestamp, so the subsequent request is re-evaluated and data is reloaded.
        /// </summary>
        Task Reload(ICache cache);

        Task Run(ICache cache, Action<string, object> save = null);
    }

    public interface ICacheTask<T> : ICacheTask
    {
        IList<ICacheLookup<T>> CacheLookups { get; }
        ICacheTask<T> Span(int seconds);
        ICacheTask<T> Lookup(ICacheLookup<T> cacheLookup);
        ICacheTask<T> OnSave(Func<IEnumerable<T>, IEnumerable<T>> func);

    }
}