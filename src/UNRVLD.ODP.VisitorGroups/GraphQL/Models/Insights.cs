using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class Insights
    {
        [JsonProperty("engagement_rank")]
        public decimal? EngagementRank { get; set; }
        [JsonProperty("winback_zone")]
        public string WinbackZone { get; set; } = string.Empty;
        [JsonProperty("order_likelihood")]
        public string OrderLikelihood { get; set; } = string.Empty;
    }

}