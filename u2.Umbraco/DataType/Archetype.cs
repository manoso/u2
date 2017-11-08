using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Archetype.Models;
using u2.Core.Contract;

namespace u2.Umbraco.DataType
{
    internal class Archetype : IContent
    {
        private readonly IDictionary<string, string> _fields;

        public Archetype(ArchetypeFieldsetModel model)
        {
            _fields = model.Properties.ToDictionary(x => x.Alias.ToLowerInvariant(), x => x.Value.ToString());
        }

        public string Alias => string.Empty;

        public T Get<T>(string alias)
        {
            var result = Get(typeof(T), alias);

            if (result is T tResult)
                return tResult;

            return default(T);
        }

        public object Get(Type type, string alias)
        {
            string result;
            alias = alias.ToLowerInvariant();

            if (_fields.TryGetValue(alias, out result))
            {
                var converter = TypeDescriptor.GetConverter(type);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    return converter.ConvertFromString(result);
                }
            }

            return null;
        }

        public bool Has(string alias)
        {
            return _fields.ContainsKey(alias);
        }
    }
}
