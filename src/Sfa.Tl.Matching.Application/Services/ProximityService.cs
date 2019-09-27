using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProximityService : IProximityService
    {
        private readonly ISearchProvider _searchProvider;
        private readonly ILocationApiClient _locationApiClient;

        public ProximityService(ISearchProvider searchProvider, ILocationApiClient locationApiClient)
        {
            _searchProvider = searchProvider;
            _locationApiClient = locationApiClient;
        }

        public async Task<IList<SearchResultsViewModelItem>> SearchProvidersByPostcodeProximity(ProviderSearchParametersDto dto)
        {
            var geoLocationData = await _locationApiClient.GetGeoLocationDataAsync(dto.Postcode, true);
            dto.Latitude = geoLocationData.Latitude;
            dto.Longitude = geoLocationData.Longitude;

            var searchResults = await _searchProvider.SearchProvidersByPostcodeProximity(dto);

            var results = searchResults.Any() ? searchResults : new List<SearchResultsViewModelItem>();

            return results;
        }

        public async Task<IList<SearchResultsByRouteViewModelItem>> SearchProvidersForOtherRoutesByPostcodeProximity(ProviderSearchParametersDto dto)
        {
            var geoLocationData = await _locationApiClient.GetGeoLocationDataAsync(dto.Postcode, true);
            dto.Latitude = geoLocationData.Latitude;
            dto.Longitude = geoLocationData.Longitude;

            var searchResults = await _searchProvider.SearchProvidersForOtherRoutesByPostcodeProximity(dto);

            var results = searchResults.Any() ? searchResults : new List<SearchResultsByRouteViewModelItem>();

            return results;
        }

        public async Task<(bool, string)> IsValidPostcode(string postcode)
        {
            return await _locationApiClient.IsValidPostcodeAsync(postcode, true);
        }
    }
}