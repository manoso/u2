using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    /// <summary>
    /// Data caching accessor for a site.
    /// Each sites have their own ICache instance. 
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// The root node of a site in CMS.
        /// </summary>
        IRoot Root { get; }

        /// <summary>
        /// Fetch all objects of T type from cache.
        /// </summary>
        /// <typeparam name="T">Type to be fetched.</typeparam>
        /// <param name="key">Alias key for the type if any. It must be the same key used when saving to the cache.</param>
        /// <returns></returns>
        IEnumerable<T> Fetch<T>(string key = null);

        /// <summary>
        /// Fetch all objects of T type from cache as lookup.
        /// </summary>
        /// <typeparam name="T">Type to be fetched.</typeparam>
        /// <param name="lookup">ICacheLookup parameter the result lookup is grouped upon.</param>
        /// <returns></returns>
        ILookup<string, T> Fetch<T>(ICacheLookup<T> lookup);

        /// <summary>
        /// Fetch all objects of T type from cache asynchronously.
        /// </summary>
        /// <typeparam name="T">Type to be fetched.</typeparam>
        /// <param name="key">Alias key for the type if any. It must be the same key used when saving to the cache.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> FetchAsync<T>(string key = null);

        /// <summary>
        /// Fetch all objects of T type from cache as lookup asynchronously.
        /// </summary>
        /// <typeparam name="T">Type to be fetched.</typeparam>
        /// <param name="lookup">ICacheLookup parameter the result lookup is grouped upon.</param>
        /// <returns></returns>
        Task<ILookup<string, T>> FetchAsync<T>(ICacheLookup<T> lookup);

        /// <summary>
        /// Reload all objects of T type into the cache asynchronously.
        /// </summary>
        /// <typeparam name="T">Type to be reloaded.</typeparam>
        /// <param name="key">Alias key for the type if any. It must be the same key used when saving to the cache.</param>
        /// <returns></returns>
        Task ReloadAsync<T>(string key = null);

        /// <summary>
        /// Reload objects of all types defined in the CacheRegistry into the cache asynchronously.
        /// </summary>
        /// <returns></returns>
        Task ReloadAsync();

        /// <summary>
        /// Reload all objects of T type into the cache.
        /// </summary>
        /// <typeparam name="T">Type to be reloaded.</typeparam>
        /// <param name="key">Alias key for the type if any. It must be the same key used when saving to the cache.</param>
        /// <returns></returns>
        void Reload<T>(string key = null);

        /// <summary>
        /// Reload objects of all types defined in the CacheRegistry into the cache.
        /// </summary>
        /// <returns></returns>
        void Reload();
    }
}