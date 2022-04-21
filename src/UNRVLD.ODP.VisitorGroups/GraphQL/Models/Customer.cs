using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class Customer
    {
        [JsonProperty("observations")]
        public Observations Observations { get; set; }

        [JsonProperty("insights")]
        public Insights Insights { get; set; }

        [JsonProperty("audiences")]
        public GraphQL.Edges<Audience> Response { get; set; }
    }

    public class Insights
    {
        [JsonProperty("engagement_rank")]
        public decimal? EngagementRank { get; set; }
        [JsonProperty("winback_zone")]
        public string WinbackZone { get; set; }
        [JsonProperty("order_likelihood")]
        public string OrderLikelihood { get; set; }
    }

    public class Observations
    {
        [JsonProperty("total_revenue")]
        public decimal TotalRevenue { get; set; }
        [JsonProperty("order_count")]
        public int OrderCount { get; set; }
        [JsonProperty("AverageOrderRevenue")]
        public decimal AverageOrderRevenue { get; set; }
    }

}