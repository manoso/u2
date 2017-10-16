using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core
{
    public class MapDefer : IMapDefer
    {
        public IDictionary<Type, ITaskDefer>  Defers { get; } = new Dictionary<Type, ITaskDefer>();

        public ITaskDefer<T> For<T>() 
            where T : class, new()
        {
            var type = typeof(T);
            if (!Defers.TryGetValue(type, out ITaskDefer defer))
            {
                defer = new TaskDefer<T>();
                Defers.Add(typeof(T), defer);
            }

            return defer as ITaskDefer<T>;
        }

        public ITaskDefer For(Type type)
        {
            if (!Defers.TryGetValue(type, out ITaskDefer defer))
            {
                defer = new TaskDefer();
                Defers.Add(type, defer);
            }

            return defer;
        }

        public void Defer(IMapTask mapTask, Func<Type, string, Task<IEnumerable<object>>> task)
        {
            if (task == null || Defers.TryGetValue(mapTask.EntityType, out ITaskDefer _))
                return;

            var taskDefer = For(mapTask.EntityType);
            foreach (var modelMap in mapTask.ModelMaps)
            {
                var map = modelMap;
                var alias = map.Alias;
                taskDefer.Attach(alias, async (x, s) =>
                {
                    if (string.IsNullOrWhiteSpace(s) || x == null) return;

                    var source = await task(map.ModelType, null);

                    if (modelMap.IsMany)
                        map.Match(x, s.Split(','), source);
                    else
                        map.Match(x, s, source);
                });
            }
        }
    }
}
