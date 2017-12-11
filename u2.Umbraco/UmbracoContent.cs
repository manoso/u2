using System.Collections.Generic;
using System.Linq;

namespace u2.Umbraco
{
    /// <summary>
    /// Content metadata type contains field data from a umbraco content node.
    /// </summary>
    public class UmbracoContent : BaseContent
    {
        private const string Raw = "__raw_";
        private const string Underscore = "__";
        private const string Format = "{0}{1}";

        public override string Alias => Get<string>("alias");

        public UmbracoContent(IDictionary<string, string> fields)
        {
            Fields = fields.ToDictionary(x => x.Key.ToLowerInvariant(), x => x.Value);
        }

        protected override string GetValue(string alias)
        {
            return Fields != null && Fields.Count > 0 &&
                   (
                       Fields.TryGetValue(string.Format(Format, Raw, alias), out var value) ||
                       Fields.TryGetValue(alias, out value) ||
                       Fields.TryGetValue(string.Format(Format, Underscore, alias), out value)
                   )
                ? value
                : null;
        }

        public override bool Has(string alias)
        {
            return Fields != null && Fields.Count > 0 &&
                   (
                       Fields.ContainsKey(alias) ||
                       Fields.ContainsKey(string.Format(Format, Raw, alias)) ||
                       Fields.ContainsKey(string.Format(Format, Underscore, alias))
                   );
        }

    }
}
