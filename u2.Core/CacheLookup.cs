using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using u2.Core.Contract;
using u2.Core.Extensions;

namespace u2.Core
{
    public class CacheLookup<T> : ICacheLookup<T>
    {
        private readonly IDictionary<string, Func<T, string>> _keys = new Dictionary<string, Func<T, string>>();

        public ICacheLookup<T> Add<TP>(Expression<Func<T, TP>> expProp)
        {
            var info = expProp.ToInfo();
            var func = expProp.Compile();
            string Final(T x) => func(x).ToString();
            _keys.Add(info.Name, Final);
            return this;
        }

        public string CacheKey => $"Lookup_{typeof(T).Name}_{string.Join("_", _keys.Keys)}";

        public string GetLookupKey(T value)
        {
            return string.Join("_", _keys.Values.Select(x => x(value)));
        }
    }
}