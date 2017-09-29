using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ICacheTask
    {
        int CacheInSecs { get; }
        string TaskKey { get; }
        Delegate Task { get; }

        IDictionary<string, object> CacheItems { get; }

        bool IsExpired { get; }

        Task Run<TResult>();

        /// <summary>
        /// Updates cache tasks' timestamp to be expired. Next subsequent request will be re-evaluated and data refreshed
        /// </summary>
        Task Reload();
    }
}