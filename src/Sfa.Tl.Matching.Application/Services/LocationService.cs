using System;
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

        public LocationService(HttpClient httpClient, MatchingConfiguration matchingConfiguration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(matchingConfiguration.PostcodeRetrieverBaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> IsValidPostCode(string postCode)
        {
            var response = await _httpClient.GetAsync($"/{postCode}/validate");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<bool>();
        }

        public async Task<PostCodeLookupResultDto> GetGeoLocationData(string postCode)
        {
            var response = await _httpClient.GetAsync($"/{postCode}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<PostCodeLookupResultDto>();
        }
    }
}