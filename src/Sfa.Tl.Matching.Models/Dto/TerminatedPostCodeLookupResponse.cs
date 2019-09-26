using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class TerminatedPostcodeLookupResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("result")]
        public TerminatedPostcodeLookupResultDto Result { get; set; }
    }
}