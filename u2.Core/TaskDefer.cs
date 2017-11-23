using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core
{
    public class TaskDefer : ITaskDefer
    {
        public IList<IMapItem> Maps { get; } = new List<IMapItem>();

        public ITaskDefer Attach(string alias, Func<ICache, object, string, Task> task)
        {
            Maps.Add(new MapItem<object, string>(alias)
            {
                ActDefer = task
            });
            return this;
        }
    }

    public class TaskDefer<T> : TaskDefer, ITaskDefer<T> where T : class, new()
    {
        public ITaskDefer<T> Attach<TP>(string alias, Func<ICache, T, TP, Task> task)
        {
            Maps.Add(new MapItem<T, TP>(alias)
            {
                ActDefer = task
            });
            return this;
        }
    }
}