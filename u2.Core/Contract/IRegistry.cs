namespace u2.Core.Contract
{
    /// <summary>
    /// Registry type to register model type for both CMS mapping and caching.
    /// Use it to register model types instead of the map registry and cache registry.
    /// </summary>
    public interface IRegistry
    {
        /// <summary>
        /// Generic method to register a model type.
        /// </summary>
        /// <typeparam name="T">The model type to register.</typeparam>
        /// <param name="key">Content type alias, if null, type name is used.</param>
        /// <returns></returns>
        IMapTask<T> Register<T>(string key = null) where T : class, new();
    }
}