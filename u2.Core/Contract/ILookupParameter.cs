using System;
using System.Linq.Expressions;

namespace u2.Core.Contract
{
    public interface ILookupParameter<T>
    {
        ILookupParameter<T> Add<TP>(Expression<Func<T, TP>> expProp);
        string CacheKey { get; }
        string GetLookupKey(T value);
    }
}