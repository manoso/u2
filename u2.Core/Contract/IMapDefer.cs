using System;
using System.Collections.Generic;

namespace u2.Core.Contract
{
    public interface IMapDefer
    {
        IDictionary<Type, ITaskDefer> Defers { get; }
        ITaskDefer For(Type type);
        ITaskDefer<T> For<T>() where T : class, new();
    }
}