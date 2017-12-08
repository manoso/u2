using System;
using System.Collections.Generic;
using u2.Core.Contract;

namespace u2.Umbraco.DataType
{
    public abstract class JsonContent : IContent
    {
        protected Dictionary<string, string> Fields;

        public abstract string Alias { get; }

        public virtual T Get<T>(string alias)
        {
            var result = Get(typeof(T), alias);

            if (result is T tResult)
                return tResult;

            return default(T);
        }

        public virtual object Get(Type type, string alias)
        {
            return Fields.TryGetValue(alias.ToLowerInvariant(), out string source)
                ? source.Convert(type)
                : null;
        }

        public virtual bool Has(string alias)
        {
            return Fields.ContainsKey(alias.ToLowerInvariant());
        }
    }
}