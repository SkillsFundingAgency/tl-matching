using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;
        private readonly MatchingConfiguration _matchingConfiguration;

        public LocationService(HttpClient httpClient, MatchingConfiguration matchingConfiguration)
        {
            _httpClient = httpClient;
            _matchingConfiguration = matchingConfiguration;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> IsValidPostCode(string postCode)
        {
            var validateUrl = $"{_matchingConfiguration.PostcodeRetrieverBaseUrl}/{postCode}/validate";
            
            var response = await _httpClient.GetAsync(validateUrl);
            
            var result = await response.Content.ReadAsStringAsync();
            
            return !string.IsNullOrWhiteSpace(result) && !result.Contains("false");
        }

        public async Task<PostCodeLookupResultDto> GetGeoLocationData(string postCode)
        {
            var lookupUrl = $"{_matchingConfiguration.PostcodeRetrieverBaseUrl}/{postCode}";
            
            var responseMessage = await _httpClient.GetAsync(lookupUrl);
            
            responseMessage.EnsureSuccessStatusCode();
            
            var response = await responseMessage.Content.ReadAsAsync<PostCodeLookupResponse>();
            
            return response.result;
        }
    }
}