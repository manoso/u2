using System;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    /// <summary>
    /// Metadata type to define the mapping of a CMS content field.
    /// </summary>
    public interface IMapItem
    {
        /// <summary>
        /// Get and set the alias for the content field.
        /// </summary>
        string Alias { get; set; }

        /// <summary>
        /// Get and set the type of the content field is converted to.
        /// </summary>
        Type ContentType { get; set; }

        /// <summary>
        /// Get the converter function used to convert the field string value to the target type and returns a object.
        /// </summary>
        Func<string, object> Converter { get; }

        /// <summary>
        /// Get and set the default value used for the field if not found or empty.
        /// </summary>
        object Default { get; set; }

        /// <summary>
        /// Get the defer func.
        /// A defer func is to convert the CMS field value to an object that need to use other objects from the cache.
        /// For example: A image picker field returns the image id only, to convert it to an image object,
        /// the image saved in the cache needs to be retrieved using the image id. 
        /// </summary>
        Func<ICache, object, object, Task> Defer { get; }

        /// <summary>
        /// Get and set the property setter for the field.
        /// </summary>
        IPropertySetter Setter { get; set; }

        /// <summary>
        /// Get the mapping func for the field.
        /// A mapping func is normally use to convert a complex CMS field to its target type.
        /// For example: use a mapping func to conver a NestedContent type to a list of objects.
        /// </summary>
        Func<string, Func<IMapper, ICache, Task<object>>> Mapper { get; }

        /// <summary>
        /// Check if the field alias match the given alias.
        /// </summary>
        /// <param name="alias">The given alias to check.</param>
        /// <returns></returns>
        bool MatchAlias(string alias);
    }

    /// <summary>
    /// The generic metadata type to define the mapping of a CMS content field.
    /// </summary>
    /// <typeparam name="T">The model type the CMS content maps to.</typeparam>
    /// <typeparam name="TP">The property type of the the model type that a cmd content field maps to.</typeparam>
    public interface IMapItem<out T, TP> : IMapItem
    {
        /// <summary>
        /// Set the Defer func. The setter converts the func into Func{ICache, object, object, Task} with a wrapper,
        /// and is accessible through <see cref="IMapItem.Defer"/>'s getter.
        /// </summary>
        Func<ICache, T, TP, Task> ActDefer { set; }

        /// <summary>
        /// Set the converter function. The setter converts the func into Func{string, object} with a wrapper,
        /// and is accessible through <see cref="IMapItem.Converter"/>'s getter.
        /// </summary>
        Func<string, TP> Convert { set; }

        /// <summary>
        /// Set the mapping function. The setter converts the func into Func{string, Func{IMapper, ICache, Task{object}}} with a wrapper,
        /// and is accessible through <see cref="IMapItem.Mapper"/>'s getter.
        /// </summary>
        Func<string, Func<IMapper, ICache, Task<object>>> Map { set; }
    }
}