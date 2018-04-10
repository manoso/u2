using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using u2.Core.Contract;
using u2.Umbraco.DataType.Archetype;

namespace u2.Umbraco.DataType
{
    /// <summary>
    /// CMS helper methods.
    /// </summary>
    public static class CmsHelper
    {
        private static readonly char[] DefaultSeparator = { ',' };

        /// <summary>
        /// Extension method to split string content field into a list of T objects using the given separators.
        /// Returns a list of T objects converted from the splitted strings.
        /// </summary>
        /// <typeparam name="T">Indicate the type is converting to.</typeparam>
        /// <param name="source">The string value read from the content field.</param>
        /// <param name="separators">Separators used to split the string, if null, defalut separator ',' is used.</param>
        /// <param name="empty">The value to be returned if string parameter source is null or empty. Default is null.</param>
        /// <returns></returns>
        public static IList<T> Split<T>(this string source, char[] separators = null, IList<T> empty = null)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return empty;
            }

            if (separators == null)
                separators = DefaultSeparator;

            var list = source
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim());

            var converter = TypeDescriptor.GetConverter(typeof(T));

            return list
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(converter.ConvertFromInvariantString)
                .OfType<T>()
                .ToList();
        }

        /// <summary>
        /// Extension method to return a map function to convert a Archetype field into a list of T objects.
        /// Returns an async map function take IMapper and ICache as input parameters and IList of T as the output object.
        /// </summary>
        /// <typeparam name="T">Indicate the type is converting to.</typeparam>
        /// <param name="source">The string value read from the Archetype field.</param>
        /// <param name="empty">The value to be returned if string parameter source is null or empty. Default is null.</param>
        /// <returns></returns>
        public static Func<IMapper, ICache, Task<object>> ToArchetypes<T>(this string source, IList<T> empty = null) where T : class, new()
        {
            var model = string.IsNullOrWhiteSpace(source) ? null : JsonConvert.DeserializeObject<Model>(source);
            var contents = model?.FieldSets.Select(fieldSet => new ArchetypeContent(fieldSet));
            return async (mapper, cache) => contents == null ? empty : (await mapper.ToAsync<T>(cache, contents).ConfigureAwait(false)).ToList();
        }

        /// <summary>
        /// Extension method to return a map function to convert a NestedContent field into a list of T objects.
        /// Returns an async map function take IMapper and ICache as input parameters and IList of T as the output object.
        /// </summary>
        /// <typeparam name="T">Indicate the type is converting to.</typeparam>
        /// <param name="source">The string value read from the NestedContent field.</param>
        /// <param name="empty">The value to be returned if string parameter source is null or empty. Default is null.</param>
        /// <returns></returns>
        public static Func<IMapper, ICache, Task<object>> ToNestedContents<T>(this string source, IList<T> empty = null) where T : class, new()
        {
            var jsons = string.IsNullOrWhiteSpace(source) ? null : JArray.Parse(source);
            var contents = jsons?.Select(json => new NestedContent(json.ToString()));
            return async (mapper, cache) => contents == null ? empty : (await mapper.ToAsync<T>(cache, contents).ConfigureAwait(false)).ToList();
        }

        /// <summary>
        /// Extension method to convert a single T object into a IList of T.
        /// Returns a IList of T object with the given T object as the only item.
        /// </summary>
        /// <typeparam name="T">Indicate the type of the object.</typeparam>
        /// <param name="item">The T object to convert.</param>
        /// <returns></returns>
        public static IList<T> ListIt<T>(this T item)
        {
            return new List<T> { item };
        }

        /// <summary>
        /// Extension method to convert a ImageCropper field into a IDictionary of crop urls.
        /// Returns a IDictionary of crop urls.
        /// </summary>
        /// <param name="source">The string value read from the ImageCropper field.</param>
        /// <returns></returns>
        public static IDictionary<string, string> ToCrops(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return null;

            var model = JsonConvert.DeserializeObject<ImageCropper>(source);

            return model?.CropUrls;
        }

        /// <summary>
        /// Extension method to convert a json field into a T object.
        /// Returns the T object.
        /// </summary>
        /// <typeparam name="T">Indicate the type is converting to.</typeparam>
        /// <param name="source">The string value read from the json field.</param>
        /// <param name="empty">The value to be returned if string parameter source is null or empty. Default is null.</param>
        /// <returns></returns>
        public static T JsonTo<T>(this string source, T empty = null) where T : class, new()
        {
            if (string.IsNullOrEmpty(source))
                return empty;

            try
            {
                return JsonConvert.DeserializeObject<T>(source);
            }
            catch
            {
                return empty;
            }
        }
    }
}