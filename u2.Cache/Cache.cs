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
        void RegisterWithLookup<T>(Func<Task<IEnumerable<T>>> func, int cacheInMins = 0, params LookupParameter<T>[] lookups);
        Task Refresh(string site = null);
    }

    public class Cache : ICache
    {
        public static int DefaultCacheTimeInMin = 10;
        public static int ShortCacheTimeInMin = 5;

        private const int UseDefaultCache = 0;

        private readonly IDictionary<string, CacheRegistry> _registries = new Dictionary<string, CacheRegistry>();

        public CacheRegistry this[string key]
        {
            get => _registries[key];
            set => _registries[key] = value;
        }

        public void Register<T>(Func<Task<IEnumerable<T>>> func, int cacheInMins = 0)
        {
            cacheInMins = cacheInMins == UseDefaultCache ? DefaultCacheTimeInMin : cacheInMins;

            Register(typeof(T).FullName, func, cacheInMins);
        }

        public void Register<T>(string key, Func<Task<IEnumerable<T>>> func, int cacheInMins = 0)
        {
            cacheInMins = cacheInMins == UseDefaultCache ? DefaultCacheTimeInMin : cacheInMins;
            foreach (var registry in _registries)
            {
                registry.Value.Add(key, func, cacheInMins);
            }
        }

        public void RegisterWithLookup<T>(Func<Task<IEnumerable<T>>> func, int cacheInMins = 0, params LookupParameter<T>[] lookups)
        {
            cacheInMins = cacheInMins == UseDefaultCache ? DefaultCacheTimeInMin : cacheInMins;
            foreach (var registry in _registries)
            {
                registry.Value.Add(func, cacheInMins, lookups);
            }
        }

        public async Task Refresh(string site = null)
        {
            if (string.IsNullOrWhiteSpace(site))
            {
                foreach (var registry in _registries)
                {
                    await registry.Value.Reload();
                }
            }
            else
                await _registries[site].Reload();
        }
    }
}

