using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UNRVLD.ODP.VisitorGroups.REST.Models
{
    public class CustomerFieldsResponse
    {

        public CustomerFieldsResponse()
        {
            this.Fields = [];
        }
        [JsonPropertyName("fields")]
        public ICollection<Field> Fields { get; set; }
    }
}