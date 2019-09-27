using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix
{
    public class GoogleDistanceMatrixResponse
    {
        [JsonProperty("destination_addresses")]
        public string[] DestinationAddresses { get; set; }

        [JsonProperty("origin_addresses")]
        public string[] OriginAddresses { get; set; }

        [JsonProperty("rows")]
        public Row[] Rows { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class Row
    {
        [JsonProperty("elements")]
        public Element[] Elements { get; set; }
    }

    public class Element
    {
        [JsonProperty("distance")]
        public Distance Distance { get; set; }

        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
    
    public class Distance
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }
    }

    public class Duration
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }
    }
}
