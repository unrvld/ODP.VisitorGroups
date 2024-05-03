using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class Observations
    {
        [JsonProperty("total_revenue")]
        public decimal? TotalRevenue { get; set; }
        [JsonProperty("order_count")]
        public int? OrderCount { get; set; }
        [JsonProperty("AverageOrderRevenue")]
        public decimal? AverageOrderRevenue { get; set; }
    }

}