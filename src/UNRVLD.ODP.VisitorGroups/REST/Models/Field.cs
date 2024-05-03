
using System.Text.Json.Serialization;

namespace UNRVLD.ODP.VisitorGroups.REST.Models
{
    public class Field
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }= string.Empty;
        
        [JsonPropertyName("description")]
        public string Description { get; set; }= string.Empty;
        
        [JsonPropertyName("type")]
        public string Type { get; set; }= string.Empty;
    }
}
