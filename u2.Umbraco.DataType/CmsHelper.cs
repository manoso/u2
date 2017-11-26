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
    public static class CmsHelper
    {
        private static readonly char[] DefaultSeparatora = { ',' };

        public static IList<T> Split<T>(this string source, char[] separators = null, IList<T> empty = null)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return empty;
            }

            if (separators == null)
                separators = DefaultSeparatora;

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

        public static Func<IMapper, ICache, Task<object>> ToArchetypes<T>(this string source, IList<T> empty = null) where T : class, new()
        {
            var model = string.IsNullOrWhiteSpace(source) ? null : JsonConvert.DeserializeObject<Model>(source);
            var contents = model?.FieldSets.Select(fieldSet => new ArchetypeContent(fieldSet));
            return async (mapper, cache) => contents == null ? empty : (await mapper.ToAsync<T>(cache, contents).ConfigureAwait(false)).ToList();
        }

        public static Func<IMapper, ICache, Task<object>> ToNestedContents<T>(this string source, IList<T> empty = null) where T : class, new()
        {
            var jsons = string.IsNullOrWhiteSpace(source) ? null : JArray.Parse(source);
            var contents = jsons?.Select(json => new NestedContent(json.ToString()));
            return async (mapper, cache) => contents == null ? empty : (await mapper.ToAsync<T>(cache, contents).ConfigureAwait(false)).ToList();
        }

        public static IList<T> ListIt<T>(this T item)
        {
            return new List<T> { item };
        }

        public static object Convert(this string source, Type type)
        {
            if (type == typeof(Guid) && TryParseGuid(source, out var guid))
            {
                return guid;
            }

            if (type == typeof(string))
            {
                return source;
            }

            var converter = TypeDescriptor.GetConverter(type);

            return converter.ConvertFromString(source);
        }

        public static bool TryParseGuid(this string source, out Guid guid)
        {
            var index = source.LastIndexOf("/", StringComparison.CurrentCultureIgnoreCase);
            if (index >= 0)
                source = source.Substring(index + 1);
            return Guid.TryParse(source, out guid);
        }


        public static IDictionary<string, string> ToCrops(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return null;

            var model = JsonConvert.DeserializeObject<ImageCropper>(source);

            return model?.CropUrls;
        }

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