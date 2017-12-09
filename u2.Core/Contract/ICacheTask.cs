using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    /// <summary>
    /// To config and store metadata of a specific object type on how to cache it.
    /// </summary>
    public interface ICacheTask
    {
        /// <summary>
        /// A key to identify the cache task, it must be unique. By default it is the full name of the type.
        /// </summary>
        string TaskKey { get; }

        /// <summary>
        /// Check if data has expired in the given cache.
        /// </summary>
        /// <param name="cache">The cache to check.</param>
        /// <returns></returns>
        bool IsExpired(ICache cache);

        /// <summary>
        /// Force reload of data to the given cache.
        /// </summary>
        /// <param name="cache">The cache to reload.</param>
        /// <returns></returns>
        Task Reload(ICache cache);

        /// <summary>
        /// A thread safe method to make sure data only gets loaded once. 
        /// </summary>
        /// <param name="cache">The cache to load data.</param>
        /// <param name="save">An action of saving the data into the cache after it is retrieved from the source.</param>
        /// <returns></returns>
        Task Run(ICache cache, Action<string, object> save = null);
    }

    public interface ICacheTask<T> : ICacheTask
    {
        /// <summary>
        /// To get all the lookups in this cache task.
        /// </summary>
        IList<ICacheLookup<T>> CacheLookups { get; }

        /// <summary>
        /// Fluent api method to set cache expiry time in seconds.
        /// </summary>
        /// <param name="seconds">Cache expiry time in seconds.</param>
        /// <returns></returns>
        ICacheTask<T> Span(int seconds);

        /// <summary>
        /// Fluent api method to add a lookup in to the cache task.
        /// </summary>
        /// <param name="cacheLookup">The lookup to add.</param>
        /// <returns></returns>
        ICacheTask<T> Lookup(ICacheLookup<T> cacheLookup);

        /// <summary>
        /// Fluent api method to do preprocessing before saving data into the cache.
        /// </summary>
        /// <param name="func">The preprocessing func.</param>
        /// <returns></returns>
        ICacheTask<T> OnSave(Func<IEnumerable<T>, IEnumerable<T>> func);
    }
}