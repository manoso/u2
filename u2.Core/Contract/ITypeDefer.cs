using System;
using System.Collections.Generic;

namespace u2.Core.Contract
{
    public interface ITypeDefer
    {
        IDictionary<string, IFieldMap> Maps { get; }
        ITypeDefer Attach(string alias, Action<object, string> action);
    }

    public interface ITypeDefer<out T> : ITypeDefer where T : class, new()
    {
        ITypeDefer<T> Attach<TP>(string alias, Action<T, TP> action);
    }
}