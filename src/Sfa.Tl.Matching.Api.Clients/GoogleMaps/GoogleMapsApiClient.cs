using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Api.Clients.Extensions;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Api.Clients.GoogleMaps
{
    public class GoogleMapApiClient : IGoogleMapApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly MatchingConfiguration _matchingConfiguration;

        public GoogleMapApiClient(HttpClient httpClient, MatchingConfiguration matchingConfiguration)
        {
            _httpClient = httpClient;
            _matchingConfiguration = matchingConfiguration;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetAddressDetails(string postCode)
        {
            if (string.IsNullOrWhiteSpace(_matchingConfiguration.GoogleMapsApiKey)) return null;
            
            var lookupUrl = $"{_matchingConfiguration.GoogleMapsApiBaseUrl}place/textsearch/json?region=uk&radius=1&key={_matchingConfiguration.GoogleMapsApiKey}&query={postCode.ToLetterOrDigit()}";

            var responseMessage = await _httpClient.GetAsync(lookupUrl);

            responseMessage.EnsureSuccessStatusCode();

            var response = await responseMessage.Content.ReadAsAsync<GooglePlacesResult>();

            return response.Status != "OK" ? string.Empty : response.Results.First().FormattedAddress.Split(",").Last().Replace(postCode, string.Empty).Trim();
        }

        public Task GetJourneyDetails(string fromPostCode, string destinationPostCode)
        {
            var apiUrl = $"{_matchingConfiguration.GoogleMapsApiBaseUrl}/&Key={_matchingConfiguration.GoogleMapsApiKey}";

            return Task.CompletedTask;
        }
    }

    public class GooglePlacesResult
    {
        [JsonProperty("html_attributions")]
        public object[] HtmlAttributions { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class Result
    {
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("icon")]
        public Uri Icon { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }

    public class Geometry
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("viewport")]
        public Viewport Viewport { get; set; }
    }

    public class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public class Viewport
    {
        [JsonProperty("northeast")]
        public Location Northeast { get; set; }

        [JsonProperty("southwest")]
        public Location Southwest { get; set; }
    }

}