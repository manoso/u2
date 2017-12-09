using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface IMapper
    {
        /// <summary>
        /// Map the content to the given type.
        /// </summary>
        /// <param name="cache">The ICache instance captured in the Context or injected by IoC.</param>
        /// <param name="content">The CMS content to be mapped from.</param>
        /// <param name="type">The model type to be mapped to.</param>
        /// <param name="value">The target instance to map to. If null, a new instance is created.</param>
        /// <returns>The solved object.</returns>
        Task<object> ToAsync(ICache cache, IContent content, Type type, object value = null);

        /// <summary>
        /// Generic method to map the content to the given type.
        /// </summary>
        /// <typeparam name="T">The model type to be mapped to.</typeparam>
        /// <param name="cache">The ICache instance captured in the Context or injected by IoC.</param>
        /// <param name="content">The CMS content to be mapped from.</param>
        /// <param name="value">The target instance to map to. If null, a new instance is created.</param>
        /// <returns>The solved object. If "value" is passed in, it is solved and returned.</returns>
        Task<T> ToAsync<T>(ICache cache, IContent content, T value = null) where T : class, new();

        /// <summary>
        /// Generic method to map a collection of CMS contents to a collection of instances of the given model type.
        /// </summary>
        /// <typeparam name="T">The model type to be mapped to.</typeparam>
        /// <typeparam name="TP">The property type of the matchProp func.</typeparam>
        /// <param name="cache">The ICache instance captured in the Context or injected by IoC.</param>
        /// <param name="contents">The collection of CMS contents to be mapped from.</param>
        /// <param name="values">The target instances to map to. If null, new instances will be created.</param>
        /// <param name="matchProp">Lambda function for the matching property.</param>
        /// <param name="matchAlias">Alias of the CMS content field used for matching.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> ToAsync<T, TP>(ICache cache, IEnumerable<IContent> contents, IEnumerable<T> values = null, Func<T, TP> matchProp = null, string matchAlias = null)
            where T : class, new();

        /// <summary>
        /// Generic method to map a collection of CMS contents to a collection of instances of the given model type.
        /// </summary>
        /// <typeparam name="T">The model type to be mapped to.</typeparam>
        /// <param name="cache">The ICache instance captured in the Context or injected by IoC.</param>
        /// <param name="contents">The collection of CMS contents to be mapped from.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> ToAsync<T>(ICache cache, IEnumerable<IContent> contents)
            where T : class, new();

        /// <summary>
        /// Map a collection of CMS contents to a collection of instances of the given model type.
        /// </summary>
        /// <param name="cache">The ICache instance captured in the Context or injected by IoC.</param>
        /// <param name="type">The model type to be mapped to.</param>
        /// <param name="contents">The collection of CMS contents to be mapped from.</param>
        /// <returns></returns>
        Task<IEnumerable<object>> ToAsync(ICache cache, Type type, IEnumerable<IContent> contents);

        /// <summary>
        /// Map the content to the given type.
        /// </summary>
        /// <param name="cache">The ICache instance captured in the Context or injected by IoC.</param>
        /// <param name="content">The CMS content to be mapped from.</param>
        /// <param name="type">The model type to be mapped to.</param>
        /// <param name="value">The target instance to map to. If null, a new instance is created.</param>
        /// <returns>The solved object.</returns>
        object To(ICache cache, IContent content, Type type, object value = null);

        /// <summary>
        /// Map the content to the given type.
        /// </summary>
        /// <typeparam name="T">The model type to be mapped to.</typeparam>
        /// <param name="cache">The ICache instance captured in the Context or injected by IoC.</param>
        /// <param name="content">The CMS content to be mapped from.</param>
        /// <param name="value">The target instance to map to. If null, a new instance is created.</param>
        /// <returns>The solved object. If "value" is passed in, it is solved and returned.</returns>
        T To<T>(ICache cache, IContent content, T value = null) where T : class, new();

        /// <summary>
        /// Map a collection of CMS contents to a collection of instances of the given model type.
        /// </summary>
        /// <typeparam name="T">The model type to be mapped to.</typeparam>
        /// <typeparam name="TP">The property type of the matchProp func.</typeparam>
        /// <param name="cache">The ICache instance captured in the Context or injected by IoC.</param>
        /// <param name="contents">The collection of CMS contents to be mapped from.</param>
        /// <param name="values">The target instances to map to. If null, new instances will be created.</param>
        /// <param name="matchProp">Lambda function for the matching property.</param>
        /// <param name="matchAlias">Alias of the CMS content field used for matching.</param>
        /// <returns></returns>
        IEnumerable<T> To<T, TP>(ICache cache, IEnumerable<IContent> contents, IEnumerable<T> values = null, Func<T, TP> matchProp = null, string matchAlias = null)
            where T : class, new();

        /// <summary>
        /// Map a collection of CMS contents to a collection of instances of the given model type.
        /// </summary>
        /// <typeparam name="T">The model type to be mapped to.</typeparam>
        /// <param name="cache">The ICache instance captured in the Context or injected by IoC.</param>
        /// <param name="contents">The collection of CMS contents to be mapped from.</param>
        /// <returns></returns>
        IEnumerable<T> To<T>(ICache cache, IEnumerable<IContent> contents)
            where T : class, new();

        /// <summary>
        /// Map a collection of CMS contents to a collection of instances of the given model type.
        /// </summary>
        /// <param name="cache">The ICache instance captured in the Context or injected by IoC.</param>
        /// <param name="type">The model type to be mapped to.</param>
        /// <param name="contents">The collection of CMS contents to be mapped from.</param>
        /// <returns></returns>
        IEnumerable<object> To(ICache cache, Type type, IEnumerable<IContent> contents);
    }
}