using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderProximityService : IProviderProximityService
    {
        private readonly ISearchProvider _searchProvider;
        private readonly ILocationService _locationService;
        private readonly ICacheService _cacheService;

        public ProviderProximityService(ISearchProvider searchProvider,
            ILocationService locationService,
            ICacheService cacheService)
        {
            _searchProvider = searchProvider;
            _locationService = locationService;
            _cacheService = cacheService;
        }

        public async Task<IList<ProviderProximitySearchResultViewModelItem>> SearchProvidersByPostcodeProximityAsync(ProviderProximitySearchParametersDto dto)
        {
            var cacheKey = $"Proximity_Search_{dto.Postcode.Replace(' ', '_')}";

            var searchResults = _cacheService.Get<IList<ProviderProximitySearchResultViewModelItem>>(cacheKey);

            if (searchResults == null)
            {
                var geoLocationData = await _locationService.GetGeoLocationDataAsync(dto.Postcode, true);
                dto.Latitude = geoLocationData.Latitude;
                dto.Longitude = geoLocationData.Longitude;

                searchResults = await _searchProvider.SearchProvidersByPostcodeProximityAsync(dto);

                _cacheService.Set(cacheKey, searchResults, TimeSpan.FromSeconds(1800));
            }

            if (dto.SelectedRoutes != null
                && dto.SelectedRoutes.Count > 0
                && searchResults != null)
            {
                var filteredResults = searchResults
                    .Where(s => s.Routes.Any(r => dto.SelectedRoutes.Contains(r.RouteId)))
                    .Select(s => new ProviderProximitySearchResultViewModelItem
                    {
                        Latitude = s.Latitude,
                        Distance = s.Distance,
                        ProviderVenueTown = s.ProviderVenueTown,
                        ProviderName = s.ProviderName,
                        ProviderVenuePostcode = s.ProviderVenuePostcode,
                        ProviderVenueId = s.ProviderVenueId,
                        ProviderDisplayName = s.ProviderDisplayName,
                        ProviderVenueName = s.ProviderVenueName,
                        IsTLevelProvider = s.IsTLevelProvider,
                        Longitude = s.Longitude,
                        Routes = s.Routes
                            .Where(rt => dto.SelectedRoutes.Contains(rt.RouteId))
                    })
                    .ToList();

                return filteredResults;
            }

            return searchResults ?? new List<ProviderProximitySearchResultViewModelItem>();
        }
    }
}