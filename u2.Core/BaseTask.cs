using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using u2.Core.Contract;

namespace u2.Core
{
    public class BaseTask : IBaseTask
    {
        public IList<IMapItem> Maps { get; } = new List<IMapItem>();

        public void AddMap(IMapItem mapItem)
        {
            if (mapItem == null)
                return;
            Maps.Add(mapItem);
        }

        protected MapItem<T, TP> CreatItem<T, TP>(Expression<Func<T, TP>> property,
            string alias = null,
            Func<string, TP> mapFunc = null,
            TP defaultVal = default(TP))
        {
            return property == null
                ? null
                : new MapItem<T, TP>(alias, property)
                    {
                        Convert = mapFunc,
                        Default = defaultVal
                    };
        }

        protected MapItem<T, TP> CreatItem<T, TP>(Expression<Func<T, TP>> property,
            string alias = null,
            Func<string, Func<IMapper, IMapDefer, object>> mapFunc = null,
            TP defaultVal = default(TP))
        {
            return property == null
                ? null
                : new MapItem<T, TP>(alias, property)
                {
                    Map = mapFunc,
                    Default = defaultVal
                };
        }
    }

    public class BaseTask<T> : BaseTask, IBaseTask<T>
        where T : class, new()
    {
        public IBaseTask<T> Map<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, TP> mapFunc = null, TP defaultVal = default(TP))
        {

            var map = CreatItem(property, alias, mapFunc, defaultVal);
            if (map != null)
                AddMap(map);

            return this;
        }
    }
}