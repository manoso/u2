using System;
using System.Collections.Generic;
using System.Linq;
using u2.Core.Contract;

namespace u2.Core
{
    public class MapRegistry : IMapRegistry
    {
        private readonly IDictionary<Type, TypeMap> _entries = new Dictionary<Type, TypeMap>();
        private readonly IDictionary<Type, SimpleMap> _copies = new Dictionary<Type, SimpleMap>();

        public IRoot Root { get; }

        public MapRegistry(IRoot root)
        {
            Root = root;
        }

        public TypeMap this[Type type] => _entries[type];

        public bool Has(Type type)
        {
            return _entries.ContainsKey(type);
        }

        public SimpleMap<T> Copy<T>()
            where T : class, new()
        {
            var map = new SimpleMap<T>();
            _copies[typeof(T)] = map;

            return map;
        }

        public TypeMap<T> Register<T>()
            where T : class, new()
        {
            var map = new TypeMap<T>();
            map.All();

            var type = typeof(T);
            foreach (var key in _copies.Keys)
            {
                if (key.IsAssignableFrom(type))
                {
                    var copy = _copies[key];
                    foreach (var fieldMap in copy.Maps)
                    {
                        map.AddMap(fieldMap);
                    }
                }
            }

            _entries[type] = map;

            return map;
        }

        public TypeMap<T> For<T>()
            where T : class, new()
        {
            return _entries.TryGetValue(typeof(T), out TypeMap typeMap) ? typeMap as TypeMap<T> : null;
        }

        public TypeMap For(Type type)
        {
            return _entries.TryGetValue(type, out TypeMap typeMap) ? typeMap : null;
        }

        public Type GetType(string contentType)
        {
            var config = _entries.Values.FirstOrDefault(x => x.Alias == contentType);
            return config?.EntityType;
        }
    }
}