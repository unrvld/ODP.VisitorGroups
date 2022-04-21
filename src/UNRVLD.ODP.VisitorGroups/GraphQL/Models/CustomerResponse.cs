using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class CustomerResponse //: GraphQL.ResponseType<Customer>
    {
        [JsonProperty("customer")] 
        public Customer Customer { get; set; }
        //public override GraphQL.Edges<Customer> Response { get; set; }
    }
}