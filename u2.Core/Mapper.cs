using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u2.Core.Contract;
using u2.Core.Extensions;

namespace u2.Core
{
    public class Mapper : IMapper
    {
        private readonly IMapRegistry _registry;

        public Mapper(IMapRegistry registry)
        {
            _registry = registry;
        }

        public async Task<object> To(IContent content, Type type, object value = null, IMapDefer defer = null)
        {
            var map = _registry.For(type);
            return await Load(map, content, value, defer);
        }

        public async Task<T> To<T>(IContent content, T value = null, IMapDefer defer = null)
            where T: class, new ()
        {
            var map = _registry.For<T>();
            return await Load(map, content, value, defer) as T;
        }

        public async Task<IEnumerable<T>> To<T, TP>(IEnumerable<IContent> contents, IEnumerable<T> values = null, Func<T, TP> matchProp = null, string matchAlias = null, IMapDefer defer = null)
            where T : class, new()
        {
            var map = _registry.For<T>();
            var list = values?.ToList();

            var needMatch = list != null && list.Any() && matchProp != null && !string.IsNullOrWhiteSpace(matchAlias);

            var result = new List<T>();
            foreach (var content in contents)
            {
                T value = null;
                if (needMatch)
                {
                    value = list.FirstOrDefault(x => MatchContent(map, content, matchProp(x), matchAlias));
                }

                var item = await To(content, value, defer);
                result.Add(item);
            }
            return result;
        }

        public async Task<IEnumerable<T>> To<T>(IEnumerable<IContent> contents, IMapDefer defer = null)
            where T : class, new()
        {
            return await To<T, object>(contents, defer: defer);
        }

        public async Task<IEnumerable<object>> To(Type type, IEnumerable<IContent> contents, IMapDefer defer = null)
        {
            var result = new List<object>();
            foreach (var content in contents)
            {
                var item = await To(content, type, null, defer);
                result.Add(item);
            }
            return result;
        }

        private async Task<object> Load(ITypeMap typeMap, IContent content, object instance = null, IMapDefer defer = null)
        {
            if (typeMap == null || content == null) return null;

            var result = typeMap.Create(instance);

            if (result == null) return null;

            var maps = GetMaps(typeMap);

            object val = null;
            maps.Each(x =>
            {
                if (content.Has(x.Alias))
                {
                    val = GetContentField(x, content);
                    x.Setter?.Set(result, val);
                }

                if (x.ContentType != null)
                {
                    val = content.Has(x.Alias) ? content.Get(x.ContentType, x.Alias) : x.Default;
                }
            });

            var fields = new Dictionary<string, object>();

            if (typeMap.CmsFields.Any() && typeMap.GroupActions.Any())
            {
                foreach (var field in typeMap.CmsFields)
                {
                    fields.Add(field.Key, content.Get(field.Value, field.Key));
                }

                foreach (var ga in typeMap.GroupActions)
                {
                    var values = ga.Aliases.Select(x => fields[x]).ToList();
                    ga.Action(result, values);
                }
            }

            typeMap.Action?.Invoke(content, result);

            if (defer != null)
            {
                if (defer.Defers.TryGetValue(typeMap.EntityType, out ITypeDefer typeDefer))
                {
                    foreach (var map in typeDefer.Maps.Select(x => x.Value))
                    {
                        val = content.Get(map.ContentType, map.Alias);
                        if (map.Defer != null)
                            await map.Defer(result, val);

                    }
                }
            }

            return result;
        }

        private object GetContentField(IFieldMap map, IContent content)
        {
            var field = content.Get(map.ContentType, map.Alias);
            if (map.Setter != null)
            {
                var str = field as string;
                if (map.Converter != null && field != null)
                    field = map.Converter(str);
            }
            return field;
        }

        private IList<IFieldMap> GetMaps(ITypeMap typeMap)
        {
            var maps = new List<IFieldMap>();
            foreach (var map in typeMap.Maps)
            {
                if (map is FieldMapCopy)
                {
                    if (_registry.Has(map.ContentType))
                        maps.AddRange(_registry[map.ContentType].Maps);
                }
                else
                    maps.Add(map);
            }
            return maps;
        }

        private bool MatchContent(ITypeMap typeMap, IContent content, object value, string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
                return false;

            alias = alias.ToLowerInvariant();

            var maps = GetMaps(typeMap);
            if (maps == null || value == null) 
                return false;

            var map = maps.FirstOrDefault(x => x.MatchAlias(alias));

            object fieldValue = null;
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            if (map.ContentType != null)
            {
                fieldValue = content.Has(map.Alias) 
                    ? GetContentField(map, content)
                    : map.Default;
            }

            return value.Equals(fieldValue);
        }

    }
}
