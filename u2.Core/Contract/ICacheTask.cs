using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ICacheTask
    {
        string TaskKey { get; }

        IDictionary<string, object> CacheItems { get; }

        bool IsExpired { get; }

        /// <summary>
        /// Updates cache tasks' timestamp to be expired. Next subsequent request will be re-evaluated and data refreshed
        /// </summary>
        Task Reload();

        Task Run(Action<string, object> save = null);
    }

    public interface ICacheTask<T> : ICacheTask
    {
        IList<ICacheLookup<T>> CacheLookups { get; }
        ICacheTask<T> Span(int seconds);
        ICacheTask<T> Lookup(ICacheLookup<T> cacheLookup);
        ICacheTask<T> OnSave(Func<IEnumerable<T>, IEnumerable<T>> func);

    }
}