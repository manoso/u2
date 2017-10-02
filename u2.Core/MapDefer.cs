using System;
using System.Collections.Generic;

namespace u2.Core
{
    public class TypeDefer
    {
        public Dictionary<string, FieldMap> Maps = new Dictionary<string, FieldMap>();

        public TypeDefer Attach(string alias, Action<object, string> action)
        {
            Maps.Add(alias, new FieldMap<object, string>(alias)
            {
                ActDefer = action
            });
            return this;
        }
    }

    public class TypeDefer<T> : TypeDefer
            where T : class, new()
    {
        public TypeDefer<T> Attach<TP>(string alias, Action<T, TP> action)
        {
            Maps.Add(alias, new FieldMap<T, TP>(alias)
            {
                ActDefer = action
            });
            return this;
        }
    }

    public class MapDefer
    {
        public Dictionary<Type, TypeDefer>  Defers = new Dictionary<Type, TypeDefer>();

        public TypeDefer<T> For<T>() 
            where T : class, new()
        {
            var defer = new TypeDefer<T>();
            Defers.Add(typeof(T), defer);

            return defer;
        }

        public TypeDefer For(Type type)
        {
            var defer = new TypeDefer();
            Defers.Add(type, defer);

            return defer;
        }
    }
}
