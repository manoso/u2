using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace u2.Core.Contract
{
    /// <summary>
    /// A base interface for IMapTask.
    /// </summary>
    public interface IBaseTask
    {
        /// <summary>
        /// Get the list of IMapItem added in the map task.
        /// </summary>
        IList<IMapItem> Maps { get; }

        /// <summary>
        /// Add a IMapItem to the map task.
        /// </summary>
        /// <param name="mapItem"></param>
        void AddMap(IMapItem mapItem);
    }

    /// <summary>
    /// A base interface for IMapTask{T}.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseTask<T> : IBaseTask
        where T : class, new()
    {
        /// <summary>
        /// Fluent method to map a Umbraco field to the type TP property of type T using the given Func.
        /// </summary>
        /// <typeparam name="TP">The property type.</typeparam>
        /// <param name="property">Lambda expression for the property of type T.</param>
        /// <param name="alias">Content field alias.</param>
        /// <param name="mapFunc">The func to convert the content field string value to type TP.</param>
        /// <param name="defaultVal">Default value if the given alias or property name is not present in the content.</param>
        /// <returns></returns>
        IBaseTask<T> Map<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, TP> mapFunc = null, TP defaultVal = default(TP));
    }
}