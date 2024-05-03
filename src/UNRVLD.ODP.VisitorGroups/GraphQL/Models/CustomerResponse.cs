using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class CustomerResponse
    {
        [JsonProperty("customer")] 
        public Customer? Customer { get; set; }
    }
}