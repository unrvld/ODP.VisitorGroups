using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class AudiencesResponse : ResponseType<Audience>
    {
        [JsonProperty("audiences")]
        public override Edges<Audience>? Response { get; set;}
    }
}