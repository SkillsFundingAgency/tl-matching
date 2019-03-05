using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Data.SearchProviders
{
    public class SqlSearchProvider : ISearchProvider
    {
        private readonly ILogger<SqlSearchProvider> _logger;
        private readonly MatchingDbContext _dbContext;

        public SqlSearchProvider(ILogger<SqlSearchProvider> logger, MatchingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ProviderVenueSearchResultDto>> SearchProvidersByPostcodeProximity(string postcode, int searchRadius, int routeId)
        {
            _logger.LogInformation($"Searching for providers within radius {searchRadius} of postcode '{postcode}' with route {routeId}");

            var qualificationIds = _dbContext.QualificationRoutePathMapping.Where(qrpm => qrpm.Path.RouteId == routeId).Select(qrpm => qrpm.QualificationId).Distinct();

            return _dbContext.ProviderVenue.Where(p => p.Location.Distance(new Point(1.2, 1.2)) <= searchRadius && p.ProviderQualification.Select(pq => pq.QualificationId).Any(qId => qualificationIds.Contains(qId)))
                .Select(p => new ProviderVenueSearchResultDto
                {
                    ProviderName = p.Provider.Name,
                    Distance = decimal.Parse(p.Location.Distance(new Point(1.2, 1.2)).ToString(CultureInfo.InvariantCulture)),
                    ProviderVenueId = p.Id,
                    Postcode = p.Postcode,
                    ProviderId = p.ProviderId,
                    QualificationShortTitles = p.ProviderQualification.Select(pq => pq.Qualification.ShortTitle)
                })
                .OrderBy(dto => dto.Distance);
        }
    }
}
