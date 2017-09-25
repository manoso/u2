using Newtonsoft.Json;

namespace Cinema.Data.Cms.DataType
{
    public class Media
    {
        [JsonProperty("src")]
        public string Url { get; set; }
    }
}