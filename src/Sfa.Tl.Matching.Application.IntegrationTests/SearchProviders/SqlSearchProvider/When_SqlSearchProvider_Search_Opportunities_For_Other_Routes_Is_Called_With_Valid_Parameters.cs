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
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider
{
    public class When_SqlSearchProvider_Search_Opportunities_For_Other_Routes_Is_Called_With_Valid_Parameters : IDisposable
    {
        private readonly IEnumerable<SearchResultsByRouteViewModelItem> _results;
        private readonly MatchingDbContext _dbContext;
        private readonly ProviderVenue _providerVenue;

        public When_SqlSearchProvider_Search_Opportunities_For_Other_Routes_Is_Called_With_Valid_Parameters()
        {
            var logger = Substitute.For<ILogger<Data.SearchProviders.SqlSearchProvider>>();

            _dbContext = new TestConfiguration().GetDbContext();

            _providerVenue = new ValidProviderVenueSearchBuilder().BuildOneVenue();
            _dbContext.Add(_providerVenue);
            _dbContext.SaveChanges();
            
            var provider = new Data.SearchProviders.SqlSearchProvider(logger, _dbContext);

            _results = provider.SearchOpportunitiesForOtherRoutesByPostcodeProximityAsync(new OpportunityProximitySearchParametersDto { Postcode = "CV1 2WT", SearchRadius = 5, SelectedRouteId = 1, Latitude = "52.400997", Longitude = "-1.508122" }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Exactly_One_Result_Is_Found_Within_Search_Radius()
        {
            _results.Should().NotBeNull();
            _results.Count().Should().Be(1);

            _results.First().NumberOfResults.Should().Be(1);
            _results.First().RouteName.Should().Be("education and childcare");
        }

        public void Dispose()
        {
            var qualificationMappings = _providerVenue.ProviderQualification.SelectMany(q => q.Qualification.QualificationRouteMapping).ToList();
            var qualifications = _providerVenue.ProviderQualification.Select(q => q.Qualification).ToList();

            _dbContext.RemoveRange(_providerVenue.ProviderQualification);
            _dbContext.RemoveRange(_providerVenue);
            _dbContext.RemoveRange(_providerVenue.Provider);
            _dbContext.RemoveRange(qualificationMappings);
            _dbContext.RemoveRange(qualifications);

            _dbContext.SaveChanges();
            _dbContext.Dispose();
        }
    }
}
