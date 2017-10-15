using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace u2.Core.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> collection,
            Func<TSource, TKey> selector)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var set = new HashSet<TKey>();

            foreach (var item in collection)
            {
                if (set.Add(selector(item)))
                {
                    yield return item;
                }
            }
        }

        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in items)
                action(item);
        }

        public static IEnumerable<T> UnionOrNull<T>(this IEnumerable<T> source, IEnumerable<T> dest)
        {
            return source == null ? dest : dest == null ? source : source.Union(dest);
        }

        public static bool IsEmpty<T>(this IEnumerable<T> source, Func<T, bool> predicate = null)
        {
            return source == null || (predicate == null ? !source.Any() : source.All(predicate));
        }

        public static IEnumerable<T> EachReturn<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }

        public static IEnumerable<T> AsList<T>(this IEnumerable<T> source)
        {
            return source.ToList();
        }
    }
}
