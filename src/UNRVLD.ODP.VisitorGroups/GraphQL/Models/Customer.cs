using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class Customer
    {
        [JsonProperty("observations")]
        public Observations? Observations { get; set; }

        [JsonProperty("insights")]
        public Insights? Insights { get; set; }

        [JsonProperty("audiences")]
        public Edges<Audience>? Response { get; set; }    

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalFields { get; set; } = new Dictionary<string, JToken>();
    }

}