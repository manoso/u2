using System;
using System.Collections.Generic;

namespace u2.Core.Contract
{
    public interface IMapper
    {
        /// <summary>
        /// Map the content to the given type.
        /// </summary>
        /// <param name="content">The content to be mapped from.</param>
        /// <param name="type">The type to be mapped to.</param>
        /// <param name="value">The target object to map to. If null, a new object will be created.</param>
        /// <param name="defer">A MapDefer object define mapping actions available at runtime only.</param>
        /// <returns>The solved object.</returns>
        object To(IContent content, Type type, object value = null, IMapDefer defer = null);

        /// <summary>
        /// Map the content to the given type.
        /// </summary>
        /// <typeparam name="T">The type to be mapped to.</typeparam>
        /// <param name="content">The content to be mapped from.</param>
        /// <param name="value">The target object to map to. If null, a new object will be created.</param>
        /// <param name="defer">A MapDefer object define mapping actions available at runtime only.</param>
        /// <returns>The solved object. If "value" is passed in, it is solved and returned.</returns>
        T To<T>(IContent content, T value = null, IMapDefer defer = null) where T : class, new();

        /// <summary>
        /// Map the contents to objects of the given type.
        /// </summary>
        /// <typeparam name="T">The type to be mapped to.</typeparam>
        /// <typeparam name="TP"></typeparam>
        /// <param name="contents">The contents to be mapped from.</param>
        /// <param name="values">The target objects to map to. If null, new objects will be created.</param>
        /// <param name="matchProp">Lambda function for the matching property.</param>
        /// <param name="matchAlias">Content alias used for the matchingProperty.</param>
        /// <param name="defer">Mapping that is deferred at run time.</param>
        /// <returns></returns>
        IEnumerable<T> To<T, TP>(IEnumerable<IContent> contents, IEnumerable<T> values = null, Func<T, TP> matchProp = null, string matchAlias = null, IMapDefer defer = null)
            where T : class, new();

        IEnumerable<T> To<T>(IEnumerable<IContent> contents, IMapDefer defer = null)
            where T : class, new();

        IEnumerable<object> To(Type type, IEnumerable<IContent> contents, IMapDefer defer = null);
    }
}