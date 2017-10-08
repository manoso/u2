using System;
using System.Collections.Generic;
using u2.Core.Contract;

namespace u2.Core
{
    public class TypeDefer : ITypeDefer
    {
        public IDictionary<string, IFieldMap> Maps { get; } = new Dictionary<string, IFieldMap>();

        public ITypeDefer Attach(string alias, Action<object, string> action)
        {
            Maps.Add(alias, new FieldMap<object, string>(alias)
            {
                ActDefer = action
            });
            return this;
        }
    }

    public class TypeDefer<T> : TypeDefer, ITypeDefer<T> where T : class, new()
    {
        public ITypeDefer<T> Attach<TP>(string alias, Action<T, TP> action)
        {
            Maps.Add(alias, new FieldMap<T, TP>(alias)
            {
                ActDefer = action
            });
            return this;
        }
    }
}