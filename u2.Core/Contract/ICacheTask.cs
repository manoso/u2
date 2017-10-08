using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ICacheTask : IOnceAsync
    {
        int CacheInSecs { get; }
        string TaskKey { get; }
        Delegate Task { get; }

        IDictionary<string, object> CacheItems { get; }

        bool IsExpired { get; }

        /// <summary>
        /// Updates cache tasks' timestamp to be expired. Next subsequent request will be re-evaluated and data refreshed
        /// </summary>
        Task Reload();
    }

    public interface ICacheTask<T> : ICacheTask
    {
        ILookupParameter<T>[] LookupParameters { get; set; }
    }
}