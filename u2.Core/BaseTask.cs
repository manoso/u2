using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using u2.Core.Contract;

namespace u2.Core
{
    public class BaseTask : IBaseTask
    {
        public IList<IMapItem> Maps { get; } = new List<IMapItem>();
        public Type EntityType { get; protected set; }
        public virtual string Alias { get; set; }

        public void AddMap(IMapItem mapItem)
        {
            if (mapItem == null)
                return;

            Maps.Add(mapItem);
        }
    }

    public class BaseTask<T> : BaseTask, IBaseTask<T>
        where T : class, new()
    {
        /// <summary>
        /// Map a Umbraco property to a object property using Func. Use it to map properties from both ends that need post processing.
        /// </summary>
        /// <typeparam name="TP">Object property type (same as Umbraco property type.</typeparam>
        /// <param name="alias">Umbraco property alias.</param>
        /// <param name="property">Lambda expression for the object property, given the declaring object.</param>
        /// <param name="mapFunc">Func to convert a TI value to a TO value.</param>
        /// <param name="defaultVal">Default value if property is not present in the content.</param>
        /// <returns>This MapTask object.</returns>
        public IBaseTask<T> Map<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, TP> mapFunc = null, TP defaultVal = default(TP))
        {

            if (property != null)
            {
                var map = new MapItem<T, TP>(alias, property)
                {
                    Convert = mapFunc,
                    Default = defaultVal
                };
                AddMap(map);
            }

            return this;
        }
    }
}