using System;

namespace u2.Core.Contract
{
    /// <summary>
    /// A CMS content
    /// </summary>
    public interface IContent
    {
        /// <summary>
        /// Check if the content has a field with the given alias.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        bool Has(string alias);

        /// <summary>
        /// Get the alias of the content type.
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// Generic method to ead the field with the given alias and return as type T.
        /// </summary>
        /// <typeparam name="T">The value type to convert to.</typeparam>
        /// <param name="alias">Alias of the field.</param>
        /// <returns></returns>
        T Get<T>(string alias);

        /// <summary>
        /// Method to read the field with the given alias and return as object.
        /// </summary>
        /// <param name="type">The value type to convert to.</param>
        /// <param name="alias">Alias of the field.</param>
        /// <returns></returns>
        object Get(Type type, string alias);
    }
}
