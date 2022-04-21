using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class AudiencesResponse : GraphQL.ResponseType<Audience>
    {
        [JsonProperty("audiences")]
        public override GraphQL.Edges<Audience> Response { get;set;}
    }
}