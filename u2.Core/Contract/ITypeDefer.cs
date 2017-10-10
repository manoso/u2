using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ITypeDefer
    {
        IDictionary<string, IFieldMap> Maps { get; }
        ITypeDefer Attach(string alias, Func<object, string, Task> task);
    }

    public interface ITypeDefer<out T> : ITypeDefer where T : class, new()
    {
        ITypeDefer<T> Attach<TP>(string alias, Func<T, TP, Task> task);
    }
}