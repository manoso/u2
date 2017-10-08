using System;
using System.Collections.Generic;

namespace u2.Core.Contract
{
    public interface IMapDefer
    {
        IDictionary<Type, ITypeDefer> Defers { get; }
        ITypeDefer For(Type type);
        ITypeDefer<T> For<T>() where T : class, new();
    }
}