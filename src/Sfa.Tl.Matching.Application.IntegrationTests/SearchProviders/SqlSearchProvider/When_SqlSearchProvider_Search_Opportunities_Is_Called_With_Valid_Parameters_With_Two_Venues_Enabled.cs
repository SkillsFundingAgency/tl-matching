using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider
{
    public class When_SqlSearchProvider_Search_Opportunities_Is_Called_With_Valid_Parameters_With_Two_Venues_Enabled : IDisposable
    {
        private readonly IEnumerable<OpportunityProximitySearchResultViewModelItem> _results;
        private readonly MatchingDbContext _dbContext;
        private readonly IList<ProviderVenue> _providerVenues;

        public When_SqlSearchProvider_Search_Opportunities_Is_Called_With_Valid_Parameters_With_Two_Venues_Enabled()
        {
            var logger = Substitute.For<ILogger<Data.SearchProviders.SqlSearchProvider>>();

            _dbContext = new TestConfiguration().GetDbContext();

            _providerVenues = new ValidProviderVenueSearchBuilder().BuildWithTwoVenuesEnabled();
            _dbContext.AddRange(_providerVenues);
            _dbContext.SaveChanges();

            var provider = new Data.SearchProviders.SqlSearchProvider(logger, _dbContext);

            _results = provider.SearchOpportunitiesByPostcodeProximityAsync(new OpportunityProximitySearchParametersDto { Postcode = "CV1 2WT", SearchRadius = 5, SelectedRouteId = 7, Latitude = "52.400997", Longitude = "-1.508122" }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Exactly_One_Provider_And_Two_Venues_Found_Within_Search_Radius()
        {
            _results.Should().NotBeNull();
            _results.Select(p => p.ProviderName).Distinct().Count().Should().Be(1);
            _results.Count().Should().Be(2);
        }

        public void Dispose()
        {
            var providerQualifications = _providerVenues.SelectMany(p => p.ProviderQualification).ToList();

            var qualificationMappings = providerQualifications.SelectMany(q => q.Qualification.QualificationRouteMapping).ToList();
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
