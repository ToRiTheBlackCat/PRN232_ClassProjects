using System.Text.Json.Serialization;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Models
{
    public class ReferencePreservedList<T> where T : class
    {
        [JsonPropertyName("$id")]
        public string Id { get; set; } // Matches "$id"
        [JsonPropertyName("$values")]
        public List<T> Values { get; set; } // Matches "$values"
    }
}
