using System;
using System.Collections.Generic;
using System.Linq;
using u2.Core.Contract;

namespace u2.Core
{
    public class MapRegistry : IMapRegistry
    {
        private readonly IDictionary<Type, IMapTask> _entries = new Dictionary<Type, IMapTask>();
        private readonly IDictionary<Type, IBaseTask> _copies = new Dictionary<Type, IBaseTask>();

        public IRoot Root { get; }

        public MapRegistry(IRoot root)
        {
            Root = root;
        }

        public IMapTask this[Type type] => _entries[type];

        public bool Has(Type type)
        {
            return _entries.ContainsKey(type);
        }

        public IBaseTask<T> Copy<T>()
            where T : class, new()
        {
            var map = new BaseTask<T>();
            _copies[typeof(T)] = map;

            return map;
        }

        public IMapTask<T> Register<T>()
            where T : class, new()
        {
            var map = new MapTask<T>();
            map.All();

            var type = typeof(T);
            foreach (var key in _copies.Keys)
            {
                if (key.IsAssignableFrom(type))
                {
                    var copy = _copies[key];
                    foreach (var fieldMap in copy.Maps)
                    {
                        map.AddMap(fieldMap, true);
                    }
                }
            }

            _entries[type] = map;

            return map;
        }

        public IMapTask For<T>()
            where T : class, new()
        {
            return For(typeof(T));
        }

        public IMapTask For(Type type)
        {
            return _entries.TryGetValue(type, out IMapTask typeMap) ? typeMap : null;
        }

        public Type GetType(string contentType)
        {
            var config = _entries.Values.FirstOrDefault(x => string.Equals(x.Alias, contentType, StringComparison.InvariantCultureIgnoreCase));
            return config?.EntityType;
        }
    }
}