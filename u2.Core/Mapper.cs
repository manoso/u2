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

        public object To(ICache cache, IContent content, Type type, object value = null)
        {
            return ToAsync(cache, content, type, value).Result;
        }

        public T To<T>(ICache cache, IContent content, T value = null)
            where T : class, new()
        {
            return ToAsync(cache, content, value).Result;
        }

        public IEnumerable<T> To<T, TP>(ICache cache, IEnumerable<IContent> contents, IEnumerable<T> values = null, Func<T, TP> matchProp = null, string matchAlias = null)
            where T : class, new()
        {
            return ToAsync(cache, contents, values, matchProp, matchAlias).Result;
        }

        public IEnumerable<T> To<T>(ICache cache, IEnumerable<IContent> contents)
            where T : class, new()
        {
            return ToAsync<T>(cache, contents).Result;
        }

        public IEnumerable<object> To(ICache cache, Type type, IEnumerable<IContent> contents)
        {
            return ToAsync(cache, type, contents).Result;
        }

        public async Task<object> ToAsync(ICache cache, IContent content, Type type, object value = null)
        {
            var map = _registry.For(type);
            return await Load(cache, map, content, value).ConfigureAwait(false);
        }

        public async Task<T> ToAsync<T>(ICache cache, IContent content, T value = null)
            where T: class, new ()
        {
            var map = _registry.For<T>();
            return await Load(cache, map, content, value).ConfigureAwait(false) as T;
        }

        public async Task<IEnumerable<T>> ToAsync<T, TP>(ICache cache, IEnumerable<IContent> contents, IEnumerable<T> values = null, Func<T, TP> matchProp = null, string matchAlias = null)
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
                    foreach (var entry in list)
                    {
                        var match = await MatchContent(cache, map, content, matchProp(entry), matchAlias);
                        if (match)
                        {
                            value = entry;
                            break;
                        }
                    }
                }

                var item = await ToAsync(cache, content, value).ConfigureAwait(false);
                result.Add(item);
            }
            return result;
        }

        public async Task<IEnumerable<T>> ToAsync<T>(ICache cache, IEnumerable<IContent> contents)
            where T : class, new()
        {
            return await ToAsync<T, object>(cache, contents).ConfigureAwait(false);
        }

        public async Task<IEnumerable<object>> ToAsync(ICache cache, Type type, IEnumerable<IContent> contents)
        {
            var result = new List<object>();
            foreach (var content in contents)
            {
                var item = await ToAsync(cache, content, type, null).ConfigureAwait(false);
                result.Add(item);
            }
            return result;
        }

        private async Task<object> Load(ICache cache, IMapTask mapTask, IContent content, object instance = null)
        {
            if (mapTask == null || content == null) return null;

            var result = mapTask.Create(instance);

            if (result == null) return null;

            var maps = GetMaps(mapTask);

            object val = null;
            maps.Each(async x =>
            {
                if (content.Has(x.Alias))
                {
                    val = await GetContentField(cache, x, content);
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

            var defer = mapTask.TaskDefer;
            if (defer != null)
            {
                foreach (var map in defer.Maps)
                {
                    val = string.IsNullOrWhiteSpace(map.Alias)
                        ? null
                        : content.Get(map.ContentType, map.Alias);
                    if (map.Defer != null)
                        await map.Defer(cache, result, val).ConfigureAwait(false);
                }
            }

            return result;
        }

        private async Task<object> GetContentField(ICache cache, IMapItem mapItem, IContent content)
        {
            var field = content.Get(mapItem.ContentType, mapItem.Alias);
            if (mapItem.Setter != null)
            {
                var str = field as string;
                if (field != null)
                {
                    if (mapItem.Converter != null)
                        field = mapItem.Converter(str);
                    else if (mapItem.Mapper != null)
                        field = await mapItem.Mapper(str)(this, cache);
                }
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

        private async Task<bool> MatchContent(ICache cache, IMapTask mapTask, IContent content, object value, string alias)
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
                    ? await GetContentField(cache, map, content)
                    : map.Default;
            }

            return value.Equals(fieldValue);
        }

    }
}
