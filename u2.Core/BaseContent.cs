using System;
using System.Collections.Generic;
using System.ComponentModel;
using u2.Core.Contract;

namespace u2.Core
{
    public abstract class BaseContent : IContent
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
            alias = alias.ToLowerInvariant();
            var value = GetValue(alias);
            return value == null ? null : Convert(value, type);
        }

        public virtual bool Has(string alias)
        {
            return Fields.ContainsKey(alias.ToLowerInvariant());
        }

        /// <summary>
        /// Given the field alias, return the field value as string.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        protected virtual string GetValue(string alias)
        {
            return Fields.TryGetValue(alias, out string value) ? value : null;
        }

        /// <summary>
        /// Extension method to convert string content field into a given type object.
        /// Returns an object of the given type.
        /// </summary>
        /// <param name="source">The string value read from the content field.</param>
        /// <param name="type">Indicate the type that the string is converting to.</param>
        /// <returns></returns>
        protected object Convert(string source, Type type)
        {
            if (type == typeof(string))
                return source;

            if (string.IsNullOrWhiteSpace(source))
                return null;

            if (type == typeof(Guid) || type == typeof(Guid?))
            {
                TryParseGuid(source, out var guid);
                return guid;
            }

            if (type == typeof(bool))
            {
                if (string.Equals(source, "yes", StringComparison.InvariantCultureIgnoreCase) ||
                    string.Equals(source, "1", StringComparison.InvariantCultureIgnoreCase) ||
                    string.Equals(source, "true", StringComparison.InvariantCultureIgnoreCase))
                    return true;
                if (string.Equals(source, "no", StringComparison.InvariantCultureIgnoreCase) ||
                    string.Equals(source, "0", StringComparison.InvariantCultureIgnoreCase) ||
                    string.Equals(source, "false", StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }

            try
            {
                var converter = TypeDescriptor.GetConverter(type);
                return converter.ConvertFromString(source);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Extension method to parse the string representation of a Udi Key to the equivalent Guid struct.
        /// Returns a boolean result indicating whether the parse is successful.
        /// </summary>
        /// <param name="source">The string representation of a Udi Key.</param>
        /// <param name="guid">Output reference of the Guid struct.</param>
        /// <returns></returns>
        protected bool TryParseGuid(string source, out Guid guid)
        {
            var index = source.LastIndexOf("/", StringComparison.CurrentCultureIgnoreCase);
            if (index >= 0)
                source = source.Substring(index + 1);
            return Guid.TryParse(source, out guid);
        }
    }
}