using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
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

        public async Task<string> GetAddressDetailsAsync(string postcode)
        {
            if (string.IsNullOrWhiteSpace(_matchingConfiguration.GoogleMapsApiKey)) return null;
            
            var lookupUrl = $"{_matchingConfiguration.GoogleMapsApiBaseUrl}place/textsearch/json?region=uk&radius=1&key={_matchingConfiguration.GoogleMapsApiKey}&query={postcode.ToLetterOrDigit()}";

            var responseMessage = await _httpClient.GetAsync(lookupUrl);

            responseMessage.EnsureSuccessStatusCode();
            
            var jsonDocument = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());

            var documentRoot = jsonDocument
                .RootElement;

            var status = 
                documentRoot
                .GetProperty("status")
                .GetString();

            if (status != "OK") 
                return string.Empty;

            var town = 
                documentRoot
                .GetProperty("results")
                .EnumerateArray()
                .First()
                .GetProperty("formatted_address")
                .GetString()
                ?.Split(",")
                .Last()
                .Replace(postcode, string.Empty)
                .Trim();

            return town ?? string.Empty;
        }
    }
}