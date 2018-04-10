using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using u2.Core;

namespace u2.Umbraco.DataType
{
    /// <summary>
    /// Nested content, similar to Umbraco content, can be mapped to model types.
    /// </summary>
    public class NestedContent : BaseContent
    {
        public NestedContent(string json)
        {
            Fields = JsonConvert.DeserializeObject<Dictionary<string, string>>(json)
                .ToDictionary(pair => pair.Key.ToLowerInvariant(), pair => pair.Value);
        }

        public override string Alias => Get<string>("nccontenttypealias");
    }
}