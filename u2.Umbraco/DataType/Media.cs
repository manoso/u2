using Newtonsoft.Json;
using u2.Core;
using u2.Core.Contract;
using u2.Demo.Data;

namespace u2.Umbraco.DataType
{
    public class Media : CmsModel, IMedia
    {
        [JsonProperty("src")]
        public string Url { get; set; }
    }
}