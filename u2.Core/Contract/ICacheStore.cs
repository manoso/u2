namespace u2.Core.Contract
{
    /// <summary>
    /// An abstract accessor to the underlying cache provider.
    /// </summary>
    public interface ICacheStore
    {
        /// <summary>
        /// Get data from cache by key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// Save the data item into cache using the given key, if the key exists, the existing data item is replaced.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="item">The data item to cache.</param>
        void Save(string key, object item);

        /// <summary>
        /// Clear/Delete the data item associated with the key from cache.
        /// </summary>
        /// <param name="key">The cache key.</param>
        void Clear(string key);

        /// <summary>
        /// Search if a key exists in the cache.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns></returns>
        bool Has(string key);
    }
}