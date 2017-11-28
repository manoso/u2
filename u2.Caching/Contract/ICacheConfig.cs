namespace u2.Caching.Contract
{
    public interface ICacheConfig
    {
        /// <summary>
        /// The default cache time in seconds.
        /// </summary>
        int CacheInSeconds { get; }
    }
}
