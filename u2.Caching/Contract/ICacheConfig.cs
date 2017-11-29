namespace u2.Caching.Contract
{
    /// <summary>
    /// Cache configuration interface
    /// A implementation of this interface need to read the actual value from a configuration source like web.config or db etc.
    /// </summary>
    public interface ICacheConfig
    {
        /// <summary>
        /// The default cache time in seconds.
        /// </summary>
        int CacheInSeconds { get; }
    }
}
