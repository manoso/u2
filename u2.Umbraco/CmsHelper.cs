using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Archetype.Models;
using Newtonsoft.Json.Linq;
using u2.Core.Contract;

namespace u2.Umbraco
{
    using DataType;
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

        public static IList<T> Archetype<T>(this string source, IMapper mapper, IList<T> empty = null) where T : class, new()
        {
            if (string.IsNullOrEmpty(source))
                return empty;

            var model = JsonConvert.DeserializeObject<ArchetypeModel>(source);

            return empty;
            //return model.Fieldsets.Any()
            //    ? model.Fieldsets.Select(x => mapper.To<T>(new Archetype(x))).ToList()
            //    : empty;
        }

        public static IList<T> NestedContents<T>(this string source, IMapper mapper) where T : class, new()
        {
            var jsons = JArray.Parse(source);
            var contents = jsons.Select(json => new NestedContent(json.ToString()));
            return mapper.To<T>(contents).ToList();
        }

        public static T NestedContent<T>(this string source, IMapper mapper, T empty = null) where T : class, new()
        {
            if (string.IsNullOrEmpty(source))
                return null;

            var content = new NestedContent(source);
            var result = mapper.To<T>(content);

            return result;
        }

        public static T JsonTo<T>(this string source) where T : class, new()
        {
            if (string.IsNullOrEmpty(source))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<T>(source);
            }
            catch
            {
                return null;
            }
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