using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace u2.Cache
{

    public interface ICache
    {
        void Register<T>(Func<Task<IEnumerable<T>>> func, int cacheInMins = 0);
        void Register<T>(string key, Func<Task<IEnumerable<T>>> func, int cacheInMins = 0);
        void RegisterLookup<T>(Func<Task<IEnumerable<T>>> func, int cacheInMins = 0, params LookupParameter<T>[] lookups);
        Task Refresh(string site = null);
    }

    public class Cache : ICache
    {
        public static int DefaultCache = 300;
        private const int UseDefault = 0;

        private readonly IDictionary<string, ICacheRegistry> _registries = new Dictionary<string, ICacheRegistry>();

        public ICacheRegistry this[string key]
        {
            get => _registries[key];
            set => _registries[key] = value;
        }

        public void Register<T>(Func<Task<IEnumerable<T>>> func, int cacheInSecs = 0)
        {
            cacheInSecs = cacheInSecs == UseDefault ? DefaultCache : cacheInSecs;
            Register(typeof(T).FullName, func, cacheInSecs);
        }

        public void Register<T>(string key, Func<Task<IEnumerable<T>>> func, int cacheInSecs = 0)
        {
            cacheInSecs = cacheInSecs == UseDefault ? DefaultCache : cacheInSecs;
            foreach (var registry in _registries.Values)
            {
                registry.Add(key, func, cacheInSecs);
            }
        }

        public void RegisterLookup<T>(Func<Task<IEnumerable<T>>> func, int cacheInSecs = 0, params LookupParameter<T>[] lookups)
        {
            cacheInSecs = cacheInSecs == UseDefault ? DefaultCache : cacheInSecs;
            foreach (var registry in _registries.Values)
            {
                registry.Add(func, cacheInSecs, lookups);
            }
        }

        public async Task Refresh(string site = null)
        {
            if (string.IsNullOrWhiteSpace(site))
            {
                foreach (var registry in _registries.Values)
                {
                    await registry.Reload();
                }
            }
            else
                await _registries[site].Reload();
        }
    }
}

