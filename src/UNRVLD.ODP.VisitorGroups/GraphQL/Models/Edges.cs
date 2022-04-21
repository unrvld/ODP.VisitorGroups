using System.Collections.Generic;
using Newtonsoft.Json;

namespace UNRVLD.ODP.VisitorGroups.GraphQL.Models
{
    public class Edges<T>
    {
        [JsonProperty("edges")]
        public List<Edge<T>> EdgeItems { get; set; }
    }


}
