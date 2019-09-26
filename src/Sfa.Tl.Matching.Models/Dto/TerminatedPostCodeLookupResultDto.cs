using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class TerminatedPostcodeLookupResultDto
    {
        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("year_terminated")]
        public string TerminatedYear { get; set; }

        [JsonProperty("month_terminated")]
        public string TerminatedMonth { get; set; }
    }
}