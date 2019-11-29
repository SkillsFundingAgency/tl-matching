using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public OpportunityProximityService(ISearchProvider searchProvider,
            ILocationService locationService)
        {
            _searchProvider = searchProvider;
            _locationService = locationService;
        }

        public async Task<IList<OpportunityProximitySearchResultViewModelItem>> SearchOpportunitiesByPostcodeProximityAsync(OpportunityProximitySearchParametersDto dto)
        {
            var geoLocationData = await _locationService.GetGeoLocationDataAsync(dto.Postcode, true);
            dto.Latitude = geoLocationData.Latitude;
            dto.Longitude = geoLocationData.Longitude;

            var searchResults = await _searchProvider.SearchOpportunitiesByPostcodeProximityAsync(dto);

            return searchResults ?? new List<OpportunityProximitySearchResultViewModelItem>();
        }

        public async Task<IList<OpportunityProximitySearchResultByRouteViewModelItem>> SearchOpportunitiesForOtherRoutesByPostcodeProximityAsync(OpportunityProximitySearchParametersDto dto)
        {
            var geoLocationData = await _locationService.GetGeoLocationDataAsync(dto.Postcode, true);
            dto.Latitude = geoLocationData.Latitude;
            dto.Longitude = geoLocationData.Longitude;

            var searchResults = await _searchProvider.SearchOpportunitiesForOtherRoutesByPostcodeProximityAsync(dto);

            var results = searchResults.Any() ? searchResults : new List<OpportunityProximitySearchResultByRouteViewModelItem>();

            return results;
        }
    }
}