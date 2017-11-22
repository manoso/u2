using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface IMapDefer
    {
        IDictionary<Type, ITaskDefer> Defers { get; }
        ITaskDefer For(Type type);
        ITaskDefer<T> For<T>() where T : class, new();
        void Defer(IMapTask mapTask, Func<Type, string, Task<IEnumerable<object>>> task);
    }
}