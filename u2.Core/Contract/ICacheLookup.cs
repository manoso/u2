using System;
using System.Linq.Expressions;

namespace u2.Core.Contract
{
    /// <summary>
    /// Define a cache lookup parameter to group T type objects by selected properties.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICacheLookup<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TP"></typeparam>
        /// <param name="expProp"></param>
        /// <returns></returns>
        ICacheLookup<T> Add<TP>(Expression<Func<T, TP>> expProp);
        string CacheKey { get; }
        string GetLookupKey(T value);
    }
}