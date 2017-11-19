using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ITaskDefer
    {
        IList<IMapItem> Maps { get; }
        ITaskDefer Attach(string alias, Func<object, string, Task> task);
    }

    public interface ITaskDefer<out T> : ITaskDefer where T : class, new()
    {
        ITaskDefer<T> Attach<TP>(string alias, Func<T, TP, Task> task);
    }
}