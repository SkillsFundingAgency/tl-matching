using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Api.Clients.Extensions;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.GeoLocations
{
    public class DummyLocationApiClient : ILocationApiClient
    {
        private readonly Dictionary<string, PostcodeLookupResultDto> _postcodeLookupData = new Dictionary<string, PostcodeLookupResultDto>
        {
            {"SW1A2AA", new PostcodeLookupResultDto {
                    Postcode = "SW1A 2AA",
                    Country = "England",
                    Region = "Region",
                    AdminCounty = null,
                    AdminDistrict = "Westminster",
                    Latitude = "51.50354",
                    Longitude = "-0.127695",
                    Outcode = "SW1A"
                }
            },
            {"CV12WT", new PostcodeLookupResultDto {
                    Postcode = "CV1 2WT",
                    Country = "England",
                    Region = "Region",
                    AdminCounty = null,
                    AdminDistrict = "Coventry",
                    Latitude = "52.400997",
                    Longitude = "-1.508122",
                    Outcode = "CV1"
                }
            }
        };

        public async Task<(bool, string)> IsValidPostcodeAsync(string postcode)
        {
            return await IsValidPostcodeAsync(postcode, false);
        }

        public async Task<(bool, string)> IsValidPostcodeAsync(string postcode, bool includeTerminated)
        {
            var postcodeWithNoSpaces = postcode.Replace(" ", string.Empty);
            return await Task.FromResult((_postcodeLookupData.ContainsKey(postcodeWithNoSpaces), postcode));
        }

        public async Task<(bool, string)> IsTerminatedPostcodeAsync(string postcode)
        {
            return await Task.FromResult((false, string.Empty));
        }

        public async Task<PostcodeLookupResultDto> GetGeoLocationDataAsync(string postcode)
        {
            return await GetGeoLocationDataAsync(postcode, false);
        }

        public async Task<PostcodeLookupResultDto> GetGeoLocationDataAsync(string postcode, bool includeTerminated)
        {
            var postcodeWithNoSpaces = postcode.Replace(" ", string.Empty);
            return await Task.FromResult(_postcodeLookupData[postcodeWithNoSpaces]);
        }

        public async Task<PostcodeLookupResultDto> GetTerminatedPostcodeGeoLocationDataAsync(string postcode)
        {
            throw new NotImplementedException();
        }
    }

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