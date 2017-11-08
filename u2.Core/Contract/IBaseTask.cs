using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace u2.Core.Contract
{
    public interface IBaseTask
    {
        IList<IMapItem> Maps { get; }

        void AddMap(IMapItem mapItem);
    }

    public interface IBaseTask<T> : IBaseTask
        where T : class, new()
    {
        /// <summary>
        /// Map the Umbraco field to the type TP property of the type T object using the given Func.
        /// </summary>
        /// <typeparam name="TP">The property type.</typeparam>
        /// <param name="property">Lambda expression for the type TP property of the type T.</param>
        /// <param name="alias">Umbraco field alias.</param>
        /// <param name="mapFunc">Func to convert the Umbraco field string value to type TP.</param>
        /// <param name="defaultVal">Default value if the given alias or property name is not present in the content.</param>
        /// <returns>The current IBaseTask object.</returns>
        IBaseTask<T> Map<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, TP> mapFunc = null, TP defaultVal = default(TP));
    }
}