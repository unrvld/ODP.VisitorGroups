#if NET5_0
using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.REST.Models
{
    public class Field
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("display_name")]
        public string display_name { get; set; }
        [JsonProperty("description")]
        public string description { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }
    }
}
#endif