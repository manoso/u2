using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core
{
    public class TaskDefer : ITaskDefer
    {
        public IDictionary<string, IMapItem> Maps { get; } = new Dictionary<string, IMapItem>();

        public ITaskDefer Attach(string alias, Func<object, string, Task> task)
        {
            Maps.Add(alias, new MapItem<object, string>(alias)
            {
                ActDefer = task
            });
            return this;
        }
    }

    public class TaskDefer<T> : TaskDefer, ITaskDefer<T> where T : class, new()
    {
        public ITaskDefer<T> Attach<TP>(string alias, Func<T, TP, Task> task)
        {
            Maps.Add(alias, new MapItem<T, TP>(alias)
            {
                ActDefer = task
            });
            return this;
        }
    }
}