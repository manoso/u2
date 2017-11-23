using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core
{
    public class MapDefer : IMapDefer
    {
        private bool _isInitialized;

        private IDictionary<Type, ITaskDefer> Defers { get; } = new Dictionary<Type, ITaskDefer>();

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

        public ITaskDefer this[IMapTask mapTask]
        {
            get
            {
                if (!_isInitialized)
                    Init(mapTask);

                return Defers.TryGetValue(mapTask.EntityType, out ITaskDefer typeDefer) ? typeDefer : null;
            }
        }

        private void Init(IMapTask mapTask)
        {
            async Task<IEnumerable<object>> Task(ICache taskCache, Type deferType, string alias)
            {
                alias = string.IsNullOrWhiteSpace(alias) ? deferType.FullName : alias;
                return await taskCache.FetchAsync<object>(alias).ConfigureAwait(false);
            }

            _isInitialized = true;

            if (Defers.TryGetValue(mapTask.EntityType, out ITaskDefer _))
                return;

            var taskDefer = For(mapTask.EntityType);
            foreach (var modelMap in mapTask.ModelMaps)
            {
                var map = modelMap;
                var alias = map.Alias;
                taskDefer.Attach(alias, async (cache, x, s) =>
                {
                    if (x == null) return;

                    var source = await Task(cache, map.ModelType, null).ConfigureAwait(false);

                    if (string.IsNullOrWhiteSpace(alias))
                        map.Match(x, source);
                    else if (!string.IsNullOrWhiteSpace(s))
                    {
                        if (modelMap.IsMany)
                            map.Match(x, s.Split(','), source);
                        else
                            map.Match(x, s, source);
                    }
                });
            }
        }
    }
}
