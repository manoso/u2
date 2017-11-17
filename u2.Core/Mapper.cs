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

        public object To(IContent content, Type type, object value = null, IMapDefer defer = null)
        {
            return ToAsync(content, type, value, defer).Result;
        }

        public T To<T>(IContent content, T value = null, IMapDefer defer = null)
            where T : class, new()
        {
            return ToAsync(content, value, defer).Result;
        }

        public IEnumerable<T> To<T, TP>(IEnumerable<IContent> contents, IEnumerable<T> values = null, Func<T, TP> matchProp = null, string matchAlias = null, IMapDefer defer = null)
            where T : class, new()
        {
            return ToAsync(contents, values, matchProp, matchAlias, defer).Result;
        }

        public IEnumerable<T> To<T>(IEnumerable<IContent> contents, IMapDefer defer = null)
            where T : class, new()
        {
            return ToAsync<T>(contents, defer).Result;
        }

        public IEnumerable<object> To(Type type, IEnumerable<IContent> contents, IMapDefer defer = null)
        {
            return ToAsync(type, contents, defer).Result;
        }

        public async Task<object> ToAsync(IContent content, Type type, object value = null, IMapDefer defer = null)
        {
            var map = _registry.For(type);
            return await Load(map, content, value, defer).ConfigureAwait(false);
        }

        public async Task<T> ToAsync<T>(IContent content, T value = null, IMapDefer defer = null)
            where T: class, new ()
        {
            var map = _registry.For<T>();
            return await Load(map, content, value, defer).ConfigureAwait(false) as T;
        }

        public async Task<IEnumerable<T>> ToAsync<T, TP>(IEnumerable<IContent> contents, IEnumerable<T> values = null, Func<T, TP> matchProp = null, string matchAlias = null, IMapDefer defer = null)
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

                var item = await ToAsync(content, value, defer).ConfigureAwait(false);
                result.Add(item);
            }
            return result;
        }

        public async Task<IEnumerable<T>> ToAsync<T>(IEnumerable<IContent> contents, IMapDefer defer = null)
            where T : class, new()
        {
            return await ToAsync<T, object>(contents, defer: defer).ConfigureAwait(false);
        }

        public async Task<IEnumerable<object>> ToAsync(Type type, IEnumerable<IContent> contents, IMapDefer defer = null)
        {
            var result = new List<object>();
            foreach (var content in contents)
            {
                var item = await ToAsync(content, type, null, defer).ConfigureAwait(false);
                result.Add(item);
            }
            return result;
        }

        private async Task<object> Load(IMapTask mapTask, IContent content, object instance = null, IMapDefer defer = null)
        {
            if (mapTask == null || content == null) return null;

            var result = mapTask.Create(instance);

            if (result == null) return null;

            var maps = GetMaps(mapTask);

            object val = null;
            maps.Each(x =>
            {
                if (content.Has(x.Alias))
                {
                    val = GetContentField(x, content);
                    x.Setter?.Set(result, val);
                }
            });

            var fields = new Dictionary<string, object>();

            if (mapTask.CmsFields.Any() && mapTask.GroupActions.Any())
            {
                foreach (var field in mapTask.CmsFields)
                {
                    fields.Add(field.Key, content.Get(field.Value, field.Key));
                }

                foreach (var ga in mapTask.GroupActions)
                {
                    var values = ga.Aliases.Select(x => fields[x]).ToList();
                    ga.Action(result, values);
                }
            }

            mapTask.Action?.Invoke(content, result);

            if (defer != null)
            {
                if (defer.Defers.TryGetValue(mapTask.EntityType, out ITaskDefer typeDefer))
                {
                    foreach (var map in typeDefer.Maps.Select(x => x.Value))
                    {
                        val = content.Get(map.ContentType, map.Alias);
                        if (map.Defer != null)
                            await map.Defer(result, val).ConfigureAwait(false);

                    }
                }
            }

            return result;
        }

        private object GetContentField(IMapItem mapItem, IContent content)
        {
            var field = content.Get(mapItem.ContentType, mapItem.Alias);
            if (mapItem.Setter != null)
            {
                var str = field as string;
                if (mapItem.Converter != null && field != null)
                    field = mapItem.Converter(str);
            }
            return field;
        }

        private IList<IMapItem> GetMaps(IMapTask mapTask)
        {
            var maps = new List<IMapItem>();
            foreach (var map in mapTask.Maps)
            {
                if (map is MapItemCopy)
                {
                    if (_registry.Has(map.ContentType))
                        maps.AddRange(_registry[map.ContentType].Maps);
                }
                else
                    maps.Add(map);
            }
            return maps;
        }

        private bool MatchContent(IMapTask mapTask, IContent content, object value, string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
                return false;

            alias = alias.ToLowerInvariant();

            var maps = GetMaps(mapTask);
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
