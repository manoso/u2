using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using u2.Core.Extensions;

namespace u2.Cache
{
    public class LookupParameter<T>
    {
        public readonly IList<PropertyInfo> Groups = new List<PropertyInfo>();

        public LookupParameter<T> Add<TP>(Expression<Func<T, TP>> expProp)
        {
            Groups.Add(expProp.ToInfo());
            return this;
        }

        public string GetKey()
        {
            var suffix = string.Join("_", Groups.Select(x => x.Name));
            return $"Lookup_{typeof(T).Name}_{suffix}";
        }

        public string GetLookupKey(T value)
        {
            return string.Join(":", Groups.Select(i => i.GetValue(value)));
        }
    }
}