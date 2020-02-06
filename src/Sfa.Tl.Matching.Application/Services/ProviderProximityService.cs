using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Extensions;
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
        private readonly IRoutePathService _routePathService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IFileWriter<ProviderProximityReportDto> _providerProximityReportWriter;

        public ProviderProximityService(ISearchProvider searchProvider,
            ILocationService locationService,
            IRoutePathService routePathService,
            IFileWriter<ProviderProximityReportDto> providerProximityReportWriter,
            IDateTimeProvider dateTimeProvider)
        {
            _searchProvider = searchProvider;
            _locationService = locationService;
            _routePathService = routePathService;
            _providerProximityReportWriter = providerProximityReportWriter;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<IList<ProviderProximitySearchResultViewModelItem>> SearchProvidersByPostcodeProximityAsync(ProviderProximitySearchParametersDto searchParameters)
        {
            var geoLocationData = await _locationService.GetGeoLocationDataAsync(searchParameters.Postcode);
            searchParameters.Latitude = geoLocationData.Latitude;
            searchParameters.Longitude = geoLocationData.Longitude;

            var searchResults = await _searchProvider.SearchProvidersByPostcodeProximityAsync(searchParameters);

            if (searchParameters.SelectedRoutes != null
                && searchParameters.SelectedRoutes.Count > 0
                && searchResults != null)
            {
                var filteredResults = searchResults
                    .Where(s => s.Routes.Any(r => searchParameters.SelectedRoutes.Contains(r.RouteId)))
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
                            .Where(rt => searchParameters.SelectedRoutes.Contains(rt.RouteId))
                    })
                    .ToList();

                return filteredResults;
            }

            return searchResults ?? new List<ProviderProximitySearchResultViewModelItem>();
        }

        public async Task<FileDownloadDto> GetProviderProximitySpreadsheetDataAsync(ProviderProximitySearchParametersDto searchParameters)
        {
            var data = await SearchProvidersByPostcodeProximityForReportAsync(searchParameters);

            var fileContent = _providerProximityReportWriter.WriteReport(data);

            var formattedDate = _dateTimeProvider.UtcNow().ToGmtStandardTime().ToString("ddMMMyyyy");

            return new FileDownloadDto
            {
                FileName = $"{searchParameters.Postcode}_searchresults_{formattedDate}.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileContent = fileContent
            };
        }

        private async Task<ProviderProximityReportDto> SearchProvidersByPostcodeProximityForReportAsync(ProviderProximitySearchParametersDto searchParameters)
        {
            var geoLocationData = await _locationService.GetGeoLocationDataAsync(searchParameters.Postcode);
            searchParameters.Latitude = geoLocationData.Latitude;
            searchParameters.Longitude = geoLocationData.Longitude;

            var searchResults = await _searchProvider.SearchProvidersByPostcodeProximityForReportAsync(searchParameters);

            if (searchParameters.SelectedRouteNames != null
                && searchParameters.SelectedRouteNames.Count > 0
                && searchResults != null)
            {
                var routes = await _routePathService.GetRouteDictionaryAsync();

                var selectedRouteIds = searchParameters.SelectedRouteNames
                    .Select(r => routes.FirstOrDefault(x => x.Value == r))
                    .Select(key => key.Key)
                    .ToList();

                var filteredProviders = searchResults.Providers
                    .Where(s => s.Routes.Any(r => selectedRouteIds.Contains(r.RouteId)))
                    .Select(s => new ProviderProximityReportItemDto
                    {
                        Distance = s.Distance,
                        ProviderVenueTown = s.ProviderVenueTown,
                        ProviderVenuePostcode = s.ProviderVenuePostcode,
                        ProviderDisplayName = s.ProviderDisplayName,
                        ProviderVenueName = s.ProviderVenueName,
                        PrimaryContact = s.PrimaryContact,
                        PrimaryContactEmail = s.PrimaryContactEmail,
                        PrimaryContactPhone = s.PrimaryContactPhone,
                        SecondaryContact = s.SecondaryContact,
                        SecondaryContactEmail = s.SecondaryContactEmail,
                        SecondaryContactPhone = s.SecondaryContactPhone,
                        Routes = s.Routes
                            .Where(rt => selectedRouteIds.Contains(rt.RouteId))
                    })
                    .ToList();

                searchResults.Providers = filteredProviders;
            }

            searchResults = searchResults ?? new ProviderProximityReportDto();
            searchResults.SkillAreas = searchParameters.SelectedRouteNames;

            return searchResults;
        }
    }
}