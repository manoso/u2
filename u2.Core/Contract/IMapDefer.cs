using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface IMapDefer
    {
        ITaskDefer this[IMapTask mapTask] { get; }
        ITaskDefer For(Type type);
        ITaskDefer<T> For<T>() where T : class, new();
    }
}