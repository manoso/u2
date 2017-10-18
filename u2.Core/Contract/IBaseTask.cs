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
        IBaseTask<T> Map<TP>(Expression<Func<T, TP>> property, string alias = null, Func<string, TP> mapFunc = null, TP defaultVal = default(TP));
    }
}