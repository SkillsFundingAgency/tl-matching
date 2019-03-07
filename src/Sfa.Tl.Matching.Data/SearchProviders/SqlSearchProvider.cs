using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GeoAPI.Geometries;
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
        private readonly ILogger<SqlSearchProvider> _logger;
        private readonly IRepository<QualificationRoutePathMapping> _qualificationRoutePathMappingRepository;
        private readonly IRepository<ProviderVenue> _providerVenueRepository;

        public SqlSearchProvider(ILogger<SqlSearchProvider> logger, IRepository<QualificationRoutePathMapping> qualificationRoutePathMappingRepository, IRepository<ProviderVenue> providerVenueRepository)
        {
            _logger = logger;
            _qualificationRoutePathMappingRepository = qualificationRoutePathMappingRepository;
            _providerVenueRepository = providerVenueRepository;
        }

        public IEnumerable<ProviderVenueSearchResultDto> SearchProvidersByPostcodeProximity(ProviderSearchParametersDto dto)
        {
            _logger.LogInformation($"Searching for providers within radius {dto.SearchRadius} of postcode '{dto.Postcode}' with route {dto.SelectedRouteId}");

            if (dto.Latitude == null || dto.Longitude == null)
                throw new InvalidOperationException("Latitude and Longitude can not be null");

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            var employerLocation = geometryFactory.CreatePoint(new Coordinate(dto.Latitude.Value, dto.Longitude.Value));

            return _providerVenueRepository
                .GetMany(p => p.Location.Distance(employerLocation) <= dto.SearchRadius && 
                              p.ProviderQualification.Select(pq => pq.QualificationId)
                                  .Any(qId => _qualificationRoutePathMappingRepository
                                      .GetMany(qrpm => qrpm.Path.RouteId == dto.SelectedRouteId)
                                      .Select(qrpm => qrpm.QualificationId)
                                      .Distinct()
                                      .Contains(qId)))
                .Select(p => new ProviderVenueSearchResultDto
                {
                    ProviderName = p.Provider.Name,
                    Distance = decimal.Parse(p.Location.Distance(employerLocation).ToString(CultureInfo.InvariantCulture)),
                    ProviderVenueId = p.Id,
                    Postcode = p.Postcode,
                    ProviderId = p.ProviderId,
                    QualificationShortTitles = p.ProviderQualification.Select(pq => pq.Qualification.ShortTitle)
                })
                .OrderBy(p => p.Distance);
        }
    }
}
