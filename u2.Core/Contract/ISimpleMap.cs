using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace u2.Core.Contract
{
    public interface ISimpleMap
    {
        string Alias { get; set; }
        Type EntityType { get; }
        IList<IFieldMap> Maps { get; }

        void AddMap(IFieldMap map);
    }
    public interface ISimpleMap<T> where T : class, new()
    {
        ISimpleMap<T> Map<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, TP> mapFunc = null, TP defaultVal = default(TP));
    }
}