using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class PopulationEstimate
    {
        [JsonProperty("estimated_lower_bound")]
        public int EstimatedLowerBound { get; set; }

        [JsonProperty("estimated_upper_bound")]
        public int EstimatedUpperBound { get; set; }
    }
}