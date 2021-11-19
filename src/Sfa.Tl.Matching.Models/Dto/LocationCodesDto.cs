// ReSharper disable UnusedMember.Global

using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class LocationCodesDto
    {
        [JsonProperty("admin_district")]
        [JsonPropertyName("admin_district")]
        public string AdminDistrict { get; set; }

        [JsonProperty("admin_county")]
        [JsonPropertyName("admin_county")]
        public string AdminCounty { get; set; }

        [JsonProperty("admin_ward")]
        [JsonPropertyName("admin_ward")]
        public string AdminWard { get; set; }

        [JsonProperty("parish")]
        [JsonPropertyName("parish")]
        public string Parish { get; set; }

        [JsonProperty("parliamentary_constituency")]
        [JsonPropertyName("parliamentary_constituency")]
        public string ParliamentaryConstituency { get; set; }

        [JsonProperty("ccg")]
        [JsonPropertyName("ccg")]
        public string Ccg { get; set; }

        [JsonProperty("ced")]
        [JsonPropertyName("ced")]
        public string Ced { get; set; }

        [JsonProperty("nuts")]
        [JsonPropertyName("nuts")]
        public string Nuts { get; set; }
    }
}