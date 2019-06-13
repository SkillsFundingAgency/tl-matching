using System.Net.Http;
using System.Net.Http.Headers;
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

        public Task<GoogleAddressDetail> GetAddressDetails(string postCode)
        {
            var apiUrl = $"{_matchingConfiguration.GoogleMapsApiBaseUrl}/{postCode.ToLetterOrDigit()}&Key={_matchingConfiguration.GoogleMapsApiKey}";

            return null;
        }

        public Task GetJourneyDetails(string fromPostCode, string destinationPostCode)
        {
            var apiUrl = $"{_matchingConfiguration.GoogleMapsApiBaseUrl}/{fromPostCode.ToLetterOrDigit()}&Key={_matchingConfiguration.GoogleMapsApiKey}";

            return Task.CompletedTask;
        }
    }

    public class GoogleAddressDetail
    {
        public string TownOrCityName { get; set; }
        public string AddressDetail { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}