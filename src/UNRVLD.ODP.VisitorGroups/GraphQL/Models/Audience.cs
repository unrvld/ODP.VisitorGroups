using Newtonsoft.Json;
using UNRVLD.ODP.VisitorGroups.GraphQL.Models.AudienceCount;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class Audience
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonProperty("population_estimate")]
        public PopulationEstimate PopulationEstimate { get; set; }
    }
}