using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Data.SearchProviders
{
    public class SqlSearchProvider : ISearchProvider
    {
        public const double MilesToMeters = 1609.34d;

        private readonly ILogger<SqlSearchProvider> _logger;
        private readonly IRepository<QualificationRoutePathMapping> _qualificationRoutePathMappingRepository;
        private readonly IRepository<ProviderVenue> _providerVenueRepository;

        public SqlSearchProvider(ILogger<SqlSearchProvider> logger, IRepository<QualificationRoutePathMapping> qualificationRoutePathMappingRepository, IRepository<ProviderVenue> providerVenueRepository)
        {
            _logger = logger;
            _qualificationRoutePathMappingRepository = qualificationRoutePathMappingRepository;
            _providerVenueRepository = providerVenueRepository;
        }

        public async Task<IEnumerable<ProviderVenueSearchResultDto>> SearchProvidersByPostcodeProximity(ProviderSearchParametersDto dto)
        {
            _logger.LogInformation($"Searching for providers within radius {dto.SearchRadius} of postcode '{dto.Postcode}' with route {dto.SelectedRouteId}");

            if (string.IsNullOrWhiteSpace(dto.Latitude) || string.IsNullOrWhiteSpace(dto.Longitude))
                throw new InvalidOperationException("Latitude and Longitude can not be null");

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            var employerLocation = geometryFactory.CreatePoint(new Coordinate(double.Parse(dto.Latitude), double.Parse(dto.Longitude)));

            var searchRadiusInMeters = dto.SearchRadius * MilesToMeters;

            var qualificationIds = await _qualificationRoutePathMappingRepository
                .GetMany(qrpm => qrpm.Path.RouteId == dto.SelectedRouteId)
                .Select(qrpm => qrpm.QualificationId)
                .Distinct().ToListAsync();

            return await _providerVenueRepository
                .GetMany(p => p.Location.Distance(employerLocation) <= searchRadiusInMeters &&
                              p.ProviderQualification.Any(qId => qualificationIds.Contains(qId.QualificationId)), inc => inc.ProviderQualification)
                .OrderBy(p => p.Location.Distance(employerLocation))
                .Select(p => new ProviderVenueSearchResultDto
                {
                    ProviderName = p.Provider.Name,
                    Distance = p.Location.Distance(employerLocation) / MilesToMeters,
                    ProviderVenueId = p.Id,
                    Postcode = p.Postcode,
                    ProviderId = p.ProviderId,
                    QualificationShortTitles = p.ProviderQualification.Select(pq => pq.Qualification.ShortTitle).ToList()
                }).ToListAsync();
        }
    }
}
