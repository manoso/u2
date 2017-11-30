using System;
using System.Linq.Expressions;

namespace u2.Core.Contract
{
    /// <summary>
    /// To group T type objects by selected properties.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICacheLookup<T>
    {
        /// <summary>
        /// Fluent api to add a lookup property, a single lookup may have multiple lookup properties.
        /// </summary>
        /// <typeparam name="TP">Property type</typeparam>
        /// <param name="expProp">Property lambda expression of given T type</param>
        /// <returns></returns>
        ICacheLookup<T> Add<TP>(Expression<Func<T, TP>> expProp);

        /// <summary>
        /// Get the cache key of the lookup. Use it to cache and fetch the lookup.
        /// </summary>
        string CacheKey { get; }

        /// <summary>
        /// Returns the lookup key of the given T object. A lookup key is used to get all the T objects that grouped underneath it.
        /// Example: for lookup ICacheLookup{Product}, added product => product.CategoryId as the single cache property, 
        /// using the key returned from GetLookupKey(new Product{ CategoryId = 1}) to retrieve all the products that have CategoryId
        /// equal to 1 from the lookup.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetLookupKey(T value);
    }
}