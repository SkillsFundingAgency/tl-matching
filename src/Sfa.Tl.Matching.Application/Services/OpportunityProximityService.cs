using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class OpportunityProximityService : IOpportunityProximityService
    {
        private readonly ISearchProvider _searchProvider;
        private readonly ILocationService _locationService;
        private readonly IGoogleDistanceMatrixApiClient _googleDistanceMatrixApiClient;

        public OpportunityProximityService(ISearchProvider searchProvider,
            ILocationService locationService,
            IGoogleDistanceMatrixApiClient googleDistanceMatrixApiClient)
        {
            _searchProvider = searchProvider;
            _locationService = locationService;
            _googleDistanceMatrixApiClient = googleDistanceMatrixApiClient;
        }

        public async Task<IList<SearchResultsViewModelItem>> SearchOpportunitiesByPostcodeProximityAsync(ProviderSearchParametersDto dto)
        {
            var geoLocationData = await _locationService.GetGeoLocationDataAsync(dto.Postcode, true);
            dto.Latitude = geoLocationData.Latitude;
            dto.Longitude = geoLocationData.Longitude;

            var searchResults = await _searchProvider.SearchOpportunitiesByPostcodeProximityAsync(dto);

            return searchResults ?? new List<SearchResultsViewModelItem>();
        }

        public async Task<IList<SearchResultsByRouteViewModelItem>> SearchOpportunitiesForOtherRoutesByPostcodeProximityAsync(ProviderSearchParametersDto dto)
        {
            var geoLocationData = await _locationService.GetGeoLocationDataAsync(dto.Postcode, true);
            dto.Latitude = geoLocationData.Latitude;
            dto.Longitude = geoLocationData.Longitude;

            var searchResults = await _searchProvider.SearchOpportunitiesForOtherRoutesByPostcodeProximityAsync(dto);

            var results = searchResults.Any() ? searchResults : new List<SearchResultsByRouteViewModelItem>();

            return results;
        }
    }
}