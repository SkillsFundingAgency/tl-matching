using System.Text.Json.Serialization;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class PostcodeLookupResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("result")]
        public PostcodeLookupResultDto Result { get; set; }
    }
}