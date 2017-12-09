using System;

namespace u2.Core.Contract
{
    /// <summary>
    /// To register CMS mapping for different model types.
    /// </summary>
    public interface IMapRegistry
    {
        /// <summary>
        /// Generic method to register a base model type for mapping from CMS. 
        /// All derived type will have the mappings defined via copy automatically.
        /// </summary>
        /// <typeparam name="T">The base type to register.</typeparam>
        /// <returns></returns>
        IBaseTask<T> Copy<T>() where T : class, new();

        /// <summary>
        /// Generic method to register a model type for mapping from CMS. 
        /// </summary>
        /// <typeparam name="T">The model type to register.</typeparam>
        /// <returns></returns>
        IMapTask<T> Register<T>() where T : class, new();

        /// <summary>
        /// Generic method to find the registered map task for the give type.
        /// </summary>
        /// <typeparam name="T">The model type to search for.</typeparam>
        /// <returns></returns>
        IMapTask For<T>() where T : class, new();

        /// <summary>
        /// To find the registered map task for the give type.
        /// </summary>
        /// <param name="type">The model type to search for.</param>
        /// <returns></returns>
        IMapTask For(Type type);

        /// <summary>
        /// Get the registered type for a given content type by alias.
        /// </summary>
        /// <param name="contentType">The content type alias.</param>
        /// <returns></returns>
        Type GetType(string contentType);

        /// <summary>
        /// Check if the given model type is registered.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool Has(Type type);

        /// <summary>
        /// Get the map task registered with the given model type.
        /// </summary>
        /// <param name="type">The given model type.</param>
        /// <returns></returns>
        IMapTask this[Type type] { get; }
    }
}