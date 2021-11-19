using System.Text.Json.Serialization;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class PostcodeLookupResultDto
    {
        [JsonPropertyName("postcode")]
        public string Postcode { get; init; }

        [JsonPropertyName("longitude")]
        public string Longitude { get; init; }

        [JsonPropertyName("latitude")]
        public string Latitude { get; init; }
    }
}