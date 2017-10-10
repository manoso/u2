using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core
{
    public class MapDefer : IMapDefer
    {
        public IDictionary<Type, ITypeDefer>  Defers { get; } = new Dictionary<Type, ITypeDefer>();

        public ITypeDefer<T> For<T>() 
            where T : class, new()
        {
            var type = typeof(T);
            if (!Defers.TryGetValue(type, out ITypeDefer defer))
            {
                defer = new TypeDefer<T>();
                Defers.Add(typeof(T), defer);
            }

            return defer as ITypeDefer<T>;
        }

        public ITypeDefer For(Type type)
        {
            if (!Defers.TryGetValue(type, out ITypeDefer defer))
            {
                defer = new TypeDefer();
                Defers.Add(type, defer);
            }

            return defer;
        }

        public void Defer(ITypeMap typeMap, Func<Type, string, Task<IEnumerable<object>>> task)
        {
            if (Defers.TryGetValue(typeMap.EntityType, out ITypeDefer _))
                return;

            var typeDefer = For(typeMap.EntityType);
            foreach (var modelMap in typeMap.ModelMaps)
            {
                var map = modelMap;
                var alias = map.Alias;
                typeDefer.Attach(alias, async (x, s) =>
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
