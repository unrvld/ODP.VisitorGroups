using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class AudienceCount
    {
        [JsonProperty("population_estimate")]
        public PopulationEstimate? PopulationEstimate { get; set; }
    }
}
