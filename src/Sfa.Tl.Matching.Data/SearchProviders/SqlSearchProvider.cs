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

        public async Task<IList<ProviderVenueSearchResultDto>> SearchProvidersByPostcodeProximity(ProviderSearchParametersDto dto)
        {
            _logger.LogInformation($"Searching for providers within radius {dto.SearchRadius} of postcode '{dto.Postcode}' with route {dto.SelectedRouteId}");

            if (string.IsNullOrWhiteSpace(dto.Latitude) || string.IsNullOrWhiteSpace(dto.Longitude))
                throw new InvalidOperationException("Latitude and Longitude can not be null");

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            var employerLocation = geometryFactory.CreatePoint(new Coordinate(double.Parse(dto.Longitude), double.Parse(dto.Latitude)));

            var searchRadiusInMeters = dto.SearchRadius * MilesToMeters;

            var result = await (from provider in _matchingDbContext.Provider
                                join providerVenue in _matchingDbContext.ProviderVenue on provider.Id equals providerVenue.ProviderId
                                join providerQualification in _matchingDbContext.ProviderQualification on providerVenue.Id equals providerQualification.ProviderVenueId
                                join qualificationRoutePathMapping in _matchingDbContext.QualificationRoutePathMapping on providerQualification.QualificationId equals qualificationRoutePathMapping.QualificationId
                                join path in _matchingDbContext.Path on qualificationRoutePathMapping.PathId equals path.Id
                                orderby providerVenue.Location.Distance(employerLocation)
                                where path.RouteId == dto.SelectedRouteId 
                                      && providerVenue.Location.Distance(employerLocation) <= searchRadiusInMeters 
                                      && provider.IsEnabledForSearch
                                      && providerVenue.IsEnabledForSearch
                                select new
                                {
                                    ProviderVenueId = providerVenue.Id,
                                    ProviderName = provider.Name,
                                    Distance = providerVenue.Location.Distance(employerLocation) / MilesToMeters,
                                    providerVenue.Postcode
                                }).Distinct().ToListAsync();

            var venueIds = result.Select(v => v.ProviderVenueId);

            var qualificationShortTitles = await (from providerQualification in _matchingDbContext.ProviderQualification
                                           join routePathMapping in _matchingDbContext.QualificationRoutePathMapping on providerQualification.QualificationId equals routePathMapping.QualificationId
                                           join qualification in _matchingDbContext.Qualification on routePathMapping.QualificationId equals qualification.Id 
                                           join path1 in _matchingDbContext.Path on routePathMapping.PathId equals path1.Id
                                           where path1.RouteId == dto.SelectedRouteId && venueIds.Any(venueId => venueId == providerQualification.ProviderVenueId)
                                           select new
                                           {
                                               providerQualification.ProviderVenueId,
                                               QualificationShortTitle = qualification.ShortTitle
                                           }).Distinct().ToListAsync();

            return result.Select(r => new ProviderVenueSearchResultDto
            {
                Postcode = r.Postcode,
                ProviderVenueId = r.ProviderVenueId,
                ProviderName = r.ProviderName,
                Distance = r.Distance,
                QualificationShortTitles = qualificationShortTitles.Where(q => q.ProviderVenueId == r.ProviderVenueId).Select(q => q.QualificationShortTitle)
            }).OrderBy(r => r.Distance).ToList();
        }
    }
}
