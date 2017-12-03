using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core
{
    public class TaskDefer : ITaskDefer
    {
        public IList<IMapItem> Maps { get; } = new List<IMapItem>();

        private void Attach(string alias, Func<ICache, object, string, Task> task)
        {
            Maps.Add(new MapItem<object, string>(alias)
            {
                ActDefer = task
            });
        }

        public TaskDefer(IEnumerable<IModelMap> modelMaps)
        {
            async Task<IEnumerable<object>> Task(ICache cache, Type deferType, string alias)
            {
                alias = string.IsNullOrWhiteSpace(alias) ? deferType.FullName : alias;
                return await cache.FetchAsync<object>(alias).ConfigureAwait(false);
            }

            foreach (var modelMap in modelMaps)
            {
                var map = modelMap;
                var alias = map.Alias;
                Attach(alias, async (cache, x, s) =>
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