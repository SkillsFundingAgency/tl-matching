using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class PostcodeLookupResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("result")]
        public PostcodeLookupResultDto Result { get; set; }
    }
}