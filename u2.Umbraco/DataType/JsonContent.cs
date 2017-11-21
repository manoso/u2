using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using u2.Core.Contract;

namespace u2.Umbraco.DataType
{
    internal abstract class JsonContent : IContent
    {
        private readonly Dictionary<string, string> _fields;

        public abstract string Alias { get; }

        protected JsonContent(string json)
        {
            _fields = JsonConvert.DeserializeObject<Dictionary<string, string>>(json)
                .ToDictionary(pair => pair.Key.ToLowerInvariant(), pair => pair.Value);
        }

        public virtual T Get<T>(string alias)
        {
            var result = Get(typeof(T), alias);

            if (result is T tResult)
                return tResult;

            return default(T);
        }

        public virtual object Get(Type type, string alias)
        {
            return _fields.TryGetValue(alias.ToLowerInvariant(), out string source)
                ? source.Convert(type)
                : null;
        }

        public virtual bool Has(string alias)
        {
            return _fields.ContainsKey(alias.ToLowerInvariant());
        }
    }
}