﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Helpers
{
    public class DummyLocationApiClient : ILocationApiClient
    {
        private readonly Dictionary<string, PostcodeLookupResultDto> _postcodeLookupData = new()
        {
            {
                "SW1A2AA",
                new PostcodeLookupResultDto
                {
                    Postcode = "SW1A 2AA",
                    Latitude = "51.50354",
                    Longitude = "-0.127695"
                }
            },
            {
                "CV12WT",
                new PostcodeLookupResultDto
                {
                    Postcode = "CV1 2WT",
                    Latitude = "52.400997",
                    Longitude = "-1.508122"
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
            return await Task.FromResult<PostcodeLookupResultDto>(null);
        }
    }
}