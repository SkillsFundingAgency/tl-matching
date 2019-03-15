using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IMapper _searchResultMapper;
        private readonly ISearchProvider _searchProvider;
        private readonly ILocationService _locationService;

        public ProviderService(
            IMapper searchResultMapper,
            ISearchProvider searchProvider,
            ILocationService locationService)
        {
            _searchResultMapper = searchResultMapper;
            _searchProvider = searchProvider;
            _locationService = locationService;
        }

        public async Task<IList<ProviderVenueSearchResultDto>> SearchProvidersByPostcodeProximity(ProviderSearchParametersDto dto)
        {
            var geoLocationData = await _locationService.GetGeoLocationData(dto.Postcode);
            dto.Latitude = geoLocationData.Latitude;
            dto.Longitude = geoLocationData.Longitude;

            var searchResults = await _searchProvider.SearchProvidersByPostcodeProximity(dto);

            var results = searchResults.Any() ? searchResults : new List<ProviderVenueSearchResultDto>();

            return results;
        }

        public async Task<bool> IsValidPostCode(string postCode)
        {
           return await _locationService.IsValidPostCode(postCode);
        }
    }
}