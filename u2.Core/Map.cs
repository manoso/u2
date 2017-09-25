using System;
using System.Collections.Generic;
using System.Linq;
using u2.Core.Extensions;

namespace u2.Core
{
    public interface IRegistry
    {
        TypeMap Register(Type type);
        TypeMap<T> Register<T>() where T : class, new();
    }

    public interface IMap
    {
        /// <summary>
        /// Map the content to the given type.
        /// </summary>
        /// <param name="content">The content to be mapped from.</param>
        /// <param name="type">The type to be mapped to.</param>
        /// <param name="value">The target object to map to. If null, a new object will be created.</param>
        /// <param name="defer">A MapDefer object define mapping actions available at runtime only.</param>
        /// <returns>The solved object.</returns>
        object To(IContent content, Type type, object value = null, MapDefer defer = null);

        /// <summary>
        /// Map the content to the given type.
        /// </summary>
        /// <typeparam name="T">The type to be mapped to.</typeparam>
        /// <param name="content">The content to be mapped from.</param>
        /// <param name="value">The target object to map to. If null, a new object will be created.</param>
        /// <param name="defer">A MapDefer object define mapping actions available at runtime only.</param>
        /// <returns>The solved object. If "value" is passed in, it is solved and returned.</returns>
        T To<T>(IContent content, T value = null, MapDefer defer = null) where T : class, new();

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
        IEnumerable<T> To<T, TP>(IEnumerable<IContent> contents, IEnumerable<T> values = null, Func<T, TP> matchProp = null, string matchAlias = null, MapDefer defer = null)
            where T : class, new();

        IEnumerable<T> To<T>(IEnumerable<IContent> contents, MapDefer defer = null)
            where T : class, new();

        Type GetType(string contentType);
    }

    public class Map : IRegistry, IMap
    {
        private readonly Dictionary<Type, TypeMap> _entries = new Dictionary<Type, TypeMap>();

        public TypeMap<T> Register<T>()
            where T : class, new()
        {
            var map = new TypeMap<T>();
            map.All();
            _entries[typeof(T)] = map;

            return map;
        }

        public TypeMap Register(Type type)
        {
            var map = new TypeMap(type);
            map.All();
            _entries[type] = map;
            return map;
        }

        public object To(IContent content, Type type, object value = null, MapDefer defer = null)
        {
            var map = For(type);
            return Load(map, content, value, defer);
        }

        public T To<T>(IContent content, T value = null, MapDefer defer = null)
            where T: class, new ()
        {
            var map = For<T>();
            return Load(map, content, value, defer) as T;
        }

        public IEnumerable<T> To<T, TP>(IEnumerable<IContent> contents, IEnumerable<T> values = null, Func<T, TP> matchProp = null, string matchAlias = null, MapDefer defer = null)
            where T : class, new()
        {
            var map = For<T>();
            var list = values?.ToList();

            var needMatch = list != null && list.Any() && matchProp != null && !string.IsNullOrWhiteSpace(matchAlias);

            foreach (var content in contents)
            {
                T value = null;
                if (needMatch)
                {
                    value = list.FirstOrDefault(x => MatchContent(map, content, matchProp(x), matchAlias));
                }

                var result = To(content, value, defer);
                yield return result;
            }
        }

        public IEnumerable<T> To<T>(IEnumerable<IContent> contents, MapDefer defer = null)
            where T : class, new()
        {
            return To<T, object>(contents, defer: defer);
        }

        public TypeMap<T> For<T>()
            where T : class, new()
        {
            TypeMap typeMap;
            if (!_entries.TryGetValue(typeof(T), out typeMap))
                typeMap = Register<T>();
            return typeMap as TypeMap<T>;
        }

        public TypeMap For(Type type)
        {
            TypeMap typeMap;
            return _entries.TryGetValue(type, out typeMap) ? typeMap : Register(type);
        }

        public Type GetType(string contentType)
        {
            var config = _entries.Values.FirstOrDefault(x => x.Alias == contentType);
            return config?.EntityType;
        }

        private object Load(TypeMap typeMap, IContent content, object instance = null, MapDefer defer = null)
        {
            if (typeMap == null || content == null) return null;

            var result = typeMap.Validate(instance);

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
                if (defer.Defers.TryGetValue(typeMap.EntityType, out TypeDefer typeDefer))
                {
                    typeDefer.Maps.Select(x => x.Value).Each(x =>
                    {
                        val = content.Get(x.ContentType, x.Alias);
                        x.Defer?.Invoke(result, val);
                    });
                }
            }

            return result;
        }

        public object GetContentField(FieldMap map, IContent content)
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

        private IList<FieldMap> GetMaps(TypeMap typeMap)
        {
            var maps = new List<FieldMap>();
            foreach (var map in typeMap.Maps)
            {
                if (map is FieldMapCopy)
                {
                    if (_entries.ContainsKey(map.ContentType))
                        maps.AddRange(_entries[map.ContentType].Maps);
                }
                else
                    maps.Add(map);
            }
            return maps;
        }

        private bool MatchContent(TypeMap typeMap, IContent content, object value, string alias)
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
