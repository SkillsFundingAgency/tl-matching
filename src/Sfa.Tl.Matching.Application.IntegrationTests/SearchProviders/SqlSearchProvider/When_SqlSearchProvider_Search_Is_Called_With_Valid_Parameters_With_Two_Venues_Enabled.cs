using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider
{
    public class When_SqlSearchProvider_Search_Is_Called_With_Valid_Parameters_With_Two_Venues_Enabled : IDisposable
    {
        private readonly IEnumerable<ProviderVenueSearchResultDto> _results;
        private readonly MatchingDbContext _dbContext;
        private readonly IList<Domain.Models.ProviderVenue> _providerVenues;

        public When_SqlSearchProvider_Search_Is_Called_With_Valid_Parameters_With_Two_Venues_Enabled()
        {
            var logger = Substitute.For<ILogger<Data.SearchProviders.SqlSearchProvider>>();

            _dbContext = new TestConfiguration().GetDbContext();

            _providerVenues = new ValidProviderVenueSearchBuilder().BuildWithTwoVenuesEnabled();
            _dbContext.AddRange(_providerVenues);
            _dbContext.SaveChanges();

            var provider = new Data.SearchProviders.SqlSearchProvider(logger, _dbContext);

            _results = provider.SearchProvidersByPostcodeProximity(new ProviderSearchParametersDto { Postcode = "CV1 2WT", SearchRadius = 5, SelectedRouteId = 7, Latitude = "52.400997", Longitude = "-1.508122" }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Results_Should_Not_Be_Null() =>
            _results.Should().NotBeNull();

        [Fact]
        public void Then_Exactly_One_Provider_Is_Found_Within_Search_Radius() =>
            _results.Select(p => p.ProviderName).Distinct().Count().Should().Be(1);

        [Fact]
        public void Then_Exactly_Two_Provider_Venue_Are_Found_Within_Search_Radius() =>
            _results.Count().Should().Be(2);

        public void Dispose()
        {
            var providerQualifications = _providerVenues.SelectMany(p => p.ProviderQualification).ToList();

            var qualificationMappings = providerQualifications.SelectMany(q => q.Qualification.QualificationRoutePathMapping).ToList();
            var qualifications = providerQualifications.Select(p => p.Qualification).ToList();

            _dbContext.RemoveRange(providerQualifications);
            _dbContext.RemoveRange(_providerVenues);
            _dbContext.RemoveRange(_providerVenues.Select(p => p.Provider));
            _dbContext.RemoveRange(qualificationMappings);
            _dbContext.RemoveRange(qualifications);

            _dbContext.SaveChanges();
            _dbContext.Dispose();
        }
    }
}
