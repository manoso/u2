using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    /// <summary>
    /// To register types for caching
    /// </summary>
    public interface ICacheRegistry
    {
        /// <summary>
        /// Registers and returns an ICacheTask for type T for caching.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="func">The async func to get all instances of T to be cached into the given ICache instance.</param>
        /// <param name="key">The key used to save to cache, if not provided, type name of T is used by default.</param>
        /// <returns></returns>
        ICacheTask<T> Add<T>(Func<ICache, Task<IEnumerable<T>>> func, string key = null);

        /// <summary>
        /// Registers and returns an ICacheTask for type T for caching.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="func">The async func to get all instances of T to be cached.</param>
        /// <param name="key">The key used to save to cache, if not provided, type name of T is used by default.</param>
        /// <returns></returns>
        ICacheTask<T> Add<T>(Func<Task<IEnumerable<T>>> func, string key = null);

        /// <summary>
        /// Check if type T has been registered caching.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns></returns>
        bool Has<T>();

        /// <summary>
        /// Check if a key has been registered caching for any type.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns></returns>
        bool Has(string key);

        /// <summary>
        /// Try to find an ICacheTask using the given task key.
        /// If found, returns true, otherwise false;
        /// </summary>
        /// <param name="taskKey">The key to get the task.</param>
        /// <param name="task">Output parameter for the task.</param>
        /// <returns></returns>
        bool TryGetTask(string taskKey, out ICacheTask task);

        /// <summary>
        /// Reload all objects of T type into the given ICache instance asynchronously.
        /// </summary>
        /// <typeparam name="T">Type to be reloaded.</typeparam>
        /// <param name="cache">The given ICache instance.</param>
        /// <param name="key">Alias key for the type if any. It must be the same key used when saving to the cache.</param>
        /// <returns></returns>
        Task ReloadAsync<T>(ICache cache, string key = null);

        /// <summary>
        /// Reload objects of all types defined in the CacheRegistry into the given ICache instance asynchronously.
        /// </summary>
        /// <param name="cache">The given ICache instance.</param>
        /// <returns></returns>
        Task ReloadAsync(ICache cache);

        /// <summary>
        /// Reload all objects of T type into the given ICache instance.
        /// </summary>
        /// <typeparam name="T">Type to be reloaded.</typeparam>
        /// <param name="cache">The given ICache instance.</param>
        /// <param name="key">Alias key for the type if any. It must be the same key used when saving to the cache.</param>
        /// <returns></returns>
        void Reload<T>(ICache cache, string key = null);

        /// <summary>
        /// Reload objects of all types defined in the CacheRegistry into the given ICache instance.
        /// </summary>
        /// <param name="cache">The given ICache instance.</param>
        /// <returns></returns>
        void Reload(ICache cache);
    }
}