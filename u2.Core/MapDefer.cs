using System;
using System.Collections.Generic;
using u2.Core.Contract;

namespace u2.Core
{
    public class MapDefer : IMapDefer
    {
        public IDictionary<Type, ITypeDefer>  Defers { get; } = new Dictionary<Type, ITypeDefer>();

        public ITypeDefer<T> For<T>() 
            where T : class, new()
        {
            var defer = new TypeDefer<T>();
            Defers.Add(typeof(T), defer);

            return defer;
        }

        public ITypeDefer For(Type type)
        {
            var defer = new TypeDefer();
            Defers.Add(type, defer);

            return defer;
        }
    }
}
