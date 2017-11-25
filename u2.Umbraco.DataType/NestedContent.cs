using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace u2.Umbraco.DataType
{
    internal class NestedContent : JsonContent
    {
        public NestedContent(string json)
        {
            Fields = JsonConvert.DeserializeObject<Dictionary<string, string>>(json)
                .ToDictionary(pair => pair.Key.ToLowerInvariant(), pair => pair.Value);
        }

        public override string Alias => Get<string>("nccontenttypealias");
    }
}