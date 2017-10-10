using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core
{
    public class TypeDefer : ITypeDefer
    {
        public IDictionary<string, IFieldMap> Maps { get; } = new Dictionary<string, IFieldMap>();

        public ITypeDefer Attach(string alias, Func<object, string, Task> task)
        {
            Maps.Add(alias, new FieldMap<object, string>(alias)
            {
                ActDefer = task
            });
            return this;
        }
    }

    public class TypeDefer<T> : TypeDefer, ITypeDefer<T> where T : class, new()
    {
        public ITypeDefer<T> Attach<TP>(string alias, Func<T, TP, Task> task)
        {
            Maps.Add(alias, new FieldMap<T, TP>(alias)
            {
                ActDefer = task
            });
            return this;
        }
    }
}