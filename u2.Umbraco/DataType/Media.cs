using Newtonsoft.Json;

namespace u2.Umbraco.DataType
{
    public class Media
    {
        [JsonProperty("src")]
        public string Url { get; set; }
    }
}