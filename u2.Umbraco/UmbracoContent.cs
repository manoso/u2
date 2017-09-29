using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using u2.Core.Contract;

namespace u2.Umbraco
{
    public class UmbracoContent : IContent
    {
        private const string Raw = "__raw_";
        private const string Underscore = "__";
        private const string Format = "{0}{1}";

        private readonly IDictionary<string, string> _fields;

        public string Alias => Get<string>("alias");

        public UmbracoContent(IDictionary<string, string> fields)
        {
            _fields = fields.ToDictionary(x => x.Key.ToLowerInvariant(), x => x.Value);
        }

        public T Get<T>(string alias)
        {
            return (T)Get(typeof(T), alias);
        }

        public object Get(Type type, string alias)
        {
            string value;
            alias = alias.ToLowerInvariant();

            if (_fields == null || _fields.Count <= 0 ||
                (!_fields.TryGetValue(string.Format(Format, Raw, alias), out value) &&
                 !_fields.TryGetValue(alias, out value) &&
                 !_fields.TryGetValue(string.Format(Format, Underscore, alias), out value))) return null;

            if (type == typeof(string))
                return value;

            if (type == typeof(bool))
                return value != "0";

            var converter = TypeDescriptor.GetConverter(type);
            return converter.ConvertFromString(value);
        }

        public bool Has(string alias)
        {
            return _fields.ContainsKey(alias) || _fields.ContainsKey(string.Format(Format, Raw, alias)) || _fields.ContainsKey(string.Format(Format, Underscore, alias));
        }

    }
}
