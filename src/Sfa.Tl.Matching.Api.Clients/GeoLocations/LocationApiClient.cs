using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Api.Clients.Extensions;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.GeoLocations
{
    public class LocationApiClient : ILocationApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _postcodeRetrieverBaseUrl;

        public LocationApiClient(HttpClient httpClient, MatchingConfiguration matchingConfiguration)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _postcodeRetrieverBaseUrl = matchingConfiguration.PostcodeRetrieverBaseUrl.TrimEnd('/');
        }

        public async Task<(bool, string)> IsValidPostcodeAsync(string postcode, bool includeTerminated)
        {
            var (isValidPostcode, postcodeResult) = await IsValidPostcodeAsync(postcode);
            if (!isValidPostcode)
            {
                (isValidPostcode, postcodeResult) = await IsTerminatedPostcodeAsync(postcode);
            }

            return (isValidPostcode, postcodeResult);
        }

        public async Task<(bool, string)> IsValidPostcodeAsync(string postcode)
        {
            try
            {
                var postcodeLookupResultDto = await GetGeoLocationDataAsync(postcode);
                return (true, postcodeLookupResultDto.Postcode);
            }
            catch
            {
                return (false, string.Empty);
            }
        }

        public async Task<(bool, string)> IsTerminatedPostcodeAsync(string postcode)
        {
            try
            {
                var postcodeLookupResultDto = await GetTerminatedPostcodeGeoLocationDataAsync(postcode);
                return (true, postcodeLookupResultDto.Postcode);
            }
            catch
            {
                return (false, string.Empty);
            }
        }

        public async Task<PostcodeLookupResultDto> GetGeoLocationDataAsync(string postcode, bool includeTerminated)
        {
            try
            {
                return await GetGeoLocationDataAsync(postcode);
            }
            catch
            {
                if (!includeTerminated)
                    throw;
            }

            return await GetTerminatedPostcodeGeoLocationDataAsync(postcode);
        }

        public async Task<PostcodeLookupResultDto> GetGeoLocationDataAsync(string postcode)
        {
            //Postcodes.io Returns 404 for "CV12 wt" so I have removed all special characters to get best possible result
            var lookupUrl = $"{_postcodeRetrieverBaseUrl}/postcodes/{postcode.ToLetterOrDigit()}";

            var responseMessage = await _httpClient.GetAsync(lookupUrl);
            
            responseMessage.EnsureSuccessStatusCode();

            var response = await responseMessage.Content.ReadAsAsync<PostcodeLookupResponse>();

            return response.Result;
        }

        public async Task<PostcodeLookupResultDto> GetTerminatedPostcodeGeoLocationDataAsync(string postcode)
        {
            var lookupUrl = $"{_postcodeRetrieverBaseUrl}/terminated_postcodes/{postcode.ToLetterOrDigit()}";

            var responseMessage = await _httpClient.GetAsync(lookupUrl);

            responseMessage.EnsureSuccessStatusCode();

            var response = await responseMessage.Content.ReadAsAsync<PostcodeLookupResponse>();

            return response.Result;
        }
    }
}