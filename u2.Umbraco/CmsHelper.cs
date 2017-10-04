using Archetype.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using u2.Core.Contract;

namespace u2.Umbraco
{
    using DataType;
    public static class CmsHelper
    {
        public static IList<T> IdsToEntities<T, TId>(IList<TId> ids, IList<T> entities, Func<T, TId> getId, IList<T> empty = null)
        {
            if (entities == null || !entities.Any() || getId == null) return empty;
            return ids.Select(x => entities.FirstOrDefault(e => getId(e).Equals(x))).ToList();
        }

        public static IList<T> Split<T>(this string source, char[] separators, IList<T> empty = null)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return empty;
            }

            var list = source
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim());

            var converter = TypeDescriptor.GetConverter(typeof(T));

            return list
                .Select(converter.ConvertFromInvariantString)
                .OfType<T>()
                .ToList();
        }

        public static IList<T> Archetype<T>(this string source, IMapper mapper, IList<T> empty = null) where T : class, new()
        {
            if (string.IsNullOrEmpty(source))
                return empty;

            var model = JsonConvert.DeserializeObject<ArchetypeModel>(source);

            return model.Fieldsets.Any() 
                ? model.Fieldsets.Select(x => mapper.To<T>(new Archetype(x))).ToList()
                : empty;
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