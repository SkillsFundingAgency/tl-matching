using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Data.SearchProviders
{
    public class SqlSearchProvider : ISearchProvider
    {
        public const double MilesToMeters = 1609.34d;

        private readonly ILogger<SqlSearchProvider> _logger;
        private readonly MatchingDbContext _matchingDbContext;

        public SqlSearchProvider(ILogger<SqlSearchProvider> logger, MatchingDbContext matchingDbContext)
        {
            _logger = logger;
            _matchingDbContext = matchingDbContext;
        }

        public async Task<IList<SearchResultsViewModelItem>> SearchProvidersByPostcodeProximityAsync(ProviderSearchParametersDto dto)
        {
            _logger.LogInformation($"Searching for providers within radius {dto.SearchRadius} of postcode '{dto.Postcode}' with route {dto.SelectedRouteId}");

            if (string.IsNullOrWhiteSpace(dto.Latitude) || string.IsNullOrWhiteSpace(dto.Longitude))
                throw new InvalidOperationException("Latitude and Longitude can not be null");

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            var employerLocation = geometryFactory.CreatePoint(new Coordinate(double.Parse(dto.Longitude), double.Parse(dto.Latitude)));

            //TODO: If javascript on then will need to search by radius first, otherwise this is ignored
            //var searchRadius = dto.SearchRadius != 0 ? dto.SearchRadius : 25;
            //var searchRadiusInMeters = searchRadius * MilesToMeters;

            var result = await (from provider in _matchingDbContext.Provider
                                join providerVenue in _matchingDbContext.ProviderVenue on provider.Id equals providerVenue.ProviderId
                                join providerQualification in _matchingDbContext.ProviderQualification on providerVenue.Id equals providerQualification.ProviderVenueId
                                join qualificationRouteMapping in _matchingDbContext.QualificationRouteMapping on providerQualification.QualificationId equals qualificationRouteMapping.QualificationId
                                orderby providerVenue.Location.Distance(employerLocation)
                                where qualificationRouteMapping.RouteId == dto.SelectedRouteId 
                                      //TODO: Only search by distance for non-javascript search
                                      //&& providerVenue.Location.Distance(employerLocation) <= searchRadiusInMeters 
                                      && provider.IsCdfProvider
                                      && provider.IsEnabledForReferral
                                      && providerVenue.IsEnabledForReferral
									  && !providerVenue.IsRemoved                                      
                                select new
                                {
                                    ProviderVenueId = providerVenue.Id,
                                    ProviderName = provider.Name,
                                    ProviderDisplayName = provider.DisplayName,
                                    ProviderVenueName = providerVenue.Name,
                                    Distance = providerVenue.Location.Distance(employerLocation) / MilesToMeters,
                                    providerVenue.Postcode,
                                    providerVenue.Town,
                                    providerVenue.Latitude,
                                    providerVenue.Longitude,
                                    provider.IsTLevelProvider
                                }).Distinct().ToListAsync();

            var venueIds = result.Select(v => v.ProviderVenueId);

            var qualificationShortTitles = await (from providerQualification in _matchingDbContext.ProviderQualification
                                           join routePathMapping in _matchingDbContext.QualificationRouteMapping on providerQualification.QualificationId equals routePathMapping.QualificationId
                                           join qualification in _matchingDbContext.Qualification on routePathMapping.QualificationId equals qualification.Id 
                                           where routePathMapping.RouteId == dto.SelectedRouteId && venueIds.Any(venueId => venueId == providerQualification.ProviderVenueId)
                                           select new
                                           {
                                               providerQualification.ProviderVenueId,
                                               QualificationShortTitle = qualification.ShortTitle
                                           }).Distinct().ToListAsync();

            return result.Select(r => new SearchResultsViewModelItem
            {
                ProviderVenueTown = r.Town,
                ProviderVenuePostcode = r.Postcode,
                ProviderVenueId = r.ProviderVenueId,
                ProviderName = r.ProviderName,
                ProviderDisplayName = r.ProviderDisplayName,
                ProviderVenueName = r.ProviderVenueName,
                Distance = r.Distance,
                IsTLevelProvider = r.IsTLevelProvider,
                Latitude = r.Latitude ?? 0,
                Longitude = r.Longitude ?? 0,
                QualificationShortTitles = qualificationShortTitles.Where(q => q.ProviderVenueId == r.ProviderVenueId).Select(q => q.QualificationShortTitle)
            }).OrderBy(r => r.Distance).ToList();
        }

        public async Task<IList<SearchResultsByRouteViewModelItem>> SearchProvidersForOtherRoutesByPostcodeProximityAsync(ProviderSearchParametersDto dto)
        {
            _logger.LogInformation($"Searching for providers within radius {dto.SearchRadius} of postcode '{dto.Postcode}' with route other than {dto.SelectedRouteId}");

            if (string.IsNullOrWhiteSpace(dto.Latitude) || string.IsNullOrWhiteSpace(dto.Longitude))
                throw new InvalidOperationException("Latitude and Longitude can not be null");

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            var employerLocation = geometryFactory.CreatePoint(new Coordinate(double.Parse(dto.Longitude), double.Parse(dto.Latitude)));

            var searchRadiusInMeters = dto.SearchRadius * MilesToMeters;

            var result = await (from provider in _matchingDbContext.Provider
                join providerVenue in _matchingDbContext.ProviderVenue on provider.Id equals providerVenue.ProviderId
                join providerQualification in _matchingDbContext.ProviderQualification on providerVenue.Id equals providerQualification.ProviderVenueId
                join qualificationRouteMapping in _matchingDbContext.QualificationRouteMapping on providerQualification.QualificationId equals qualificationRouteMapping.QualificationId
                join route in _matchingDbContext.Route on qualificationRouteMapping.RouteId equals route.Id
                orderby route.Name
                where qualificationRouteMapping.RouteId != dto.SelectedRouteId
                      && providerVenue.Location.Distance(employerLocation) <= searchRadiusInMeters
                      && provider.IsCdfProvider
                      && provider.IsEnabledForReferral
                      && providerVenue.IsEnabledForReferral
                      && !providerVenue.IsRemoved
                select new
                {
                    ProviderVenueId = providerVenue.Id,
                    RouteName = route.Name
                }).Distinct().ToListAsync();

            return result
                .GroupBy(r => r.RouteName)
                .Select(rg => new SearchResultsByRouteViewModelItem
                {
                    RouteName = rg.Key.ToLower(),
                    NumberOfResults = rg.Count()
                }).OrderBy(r => r.RouteName).ToList();
        }
    }
}
