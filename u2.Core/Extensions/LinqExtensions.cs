using System;
using System.Collections.Generic;
using System.Linq;

namespace u2.Core.Extensions
{
    /// <summary>
    /// Custome Linq extension methods.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Fluent Linq api version of Foreach loop.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="source">The IEnumerable source.</param>
        /// <param name="action">Action to run for each item.</param>
        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
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
                action(item);
        }

        /// <summary>
        /// Return a list as IEnumerable.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="source">The IEnumerable source.</param>
        /// <returns></returns>
        public static IEnumerable<T> AsList<T>(this IEnumerable<T> source)
        {
            return source.ToList();
        }
    }
}
