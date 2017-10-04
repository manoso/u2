using System;
using System.Collections.Generic;
using u2.Core.Contract;

namespace u2.Core
{
    public class Map : IMap
    {
        private readonly IMapRegistry _registry;
        private readonly IMapper _mapper;

        public Map(IMapRegistry registry, IMapper mapper)
        {
            _registry = registry;
            _mapper = mapper;
        }

        public IRoot Root => _registry.Root;

        public SimpleMap<T> Copy<T>() where T : class, new()
        {
            return _registry.Copy<T>();
        }

        public TypeMap<T> Register<T>() where T : class, new()
        {
            return _registry.Register<T>();
        }

        public TypeMap For<T>() where T : class, new()
        {
            return _registry.For<T>();
        }

        public TypeMap For(Type type)
        {
            return _registry.For(type);
        }

        public Type GetType(string contentType)
        {
            return _registry.GetType(contentType);
        }

        public bool Has(Type type)
        {
            return _registry.Has(type);
        }

        public TypeMap this[Type type] => _registry[type];

        public object To(IContent content, Type type, object value = null, MapDefer defer = null)
        {
            return _mapper.To(content, type, value, defer);
        }

        public T To<T>(IContent content, T value = default(T), MapDefer defer = null) where T : class, new()
        {
            return _mapper.To(content, value, defer);
        }

        public IEnumerable<T> To<T, TP>(IEnumerable<IContent> contents, IEnumerable<T> values = null, Func<T, TP> matchProp = null, string matchAlias = null,
            MapDefer defer = null) where T : class, new()
        {
            return _mapper.To(contents, values, matchProp, matchAlias, defer);
        }

        public IEnumerable<T> To<T>(IEnumerable<IContent> contents, MapDefer defer = null) where T : class, new()
        {
            return _mapper.To<T>(contents, defer);
        }

        public IEnumerable<object> To(Type type, IEnumerable<IContent> contents, MapDefer defer = null)
        {
            return _mapper.To(type, contents, defer);
        }
    }
}
