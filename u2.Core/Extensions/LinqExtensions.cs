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


        /// <summary>
        /// Pick the items that has the same property value of the given source items from target
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="srcProperty"></param>
        /// <param name="tgtProperty"></param>
        /// <returns></returns>
        public static IEnumerable<TTarget> Pick<T, TTarget, TProperty>(this IEnumerable<T> source,
            IEnumerable<TTarget> target,
            Func<T, TProperty> srcProperty,
            Func<TTarget, TProperty> tgtProperty)
        {
            if (source == null || target == null)
                yield break;

            foreach (var tItem in target)
            {
                if (source.Any(s => srcProperty(s).Equals(tgtProperty(tItem))))
                    yield return tItem;
            }
        }

        public static T Load<T, TTarget, TField>(this T entity,
            Expression<Func<T, IEnumerable<TTarget>>> loadProperty,
            Func<T, IEnumerable<TField>> srcProperty,
            IEnumerable<TTarget> target,
            Func<TTarget, TField> tgtProperty)
        {
            if (loadProperty != null && srcProperty != null && entity != null && target != null && tgtProperty != null)
            {
                var propInfo = loadProperty.ToInfo();
                if (propInfo != null)
                {
                    var source = srcProperty(entity);

                    var values = source.Pick(target, x => x, tgtProperty);
                    if (values != null)
                        propInfo.SetValue(entity, values.ToList());
                }
            }
            return entity;
        }

        public static T Load<T, TTarget, TField>(this T entity,
            Func<T, TField> srcProperty,
            IEnumerable<TTarget> target,
            Func<TTarget, TField> tgtProperty,
            Action<T, TTarget> action)
        {
            if (srcProperty != null && entity != null && target != null && tgtProperty != null && action != null)
            {
                var srcVal = srcProperty(entity);

                if (srcVal != null)
                {
                    foreach (var item in target)
                    {
                        var itmVal = tgtProperty(item);
                        if (itmVal.Equals(srcVal))
                        {
                            action(entity, item);
                            break;
                        }
                    }
                }
            }
            return entity;
        }

        public static T Load<T, TTarget>(this T entity, TTarget target, Action<T, TTarget> action)
        {
            if (entity != null && target != null)
                action?.Invoke(entity, target);
            return entity;
        }

        public static IEnumerable<T> AsList<T>(this IEnumerable<T> source)
        {
            return source.ToList();
        }
    }
}
