using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider
{
    public class When_SqlSearchProvider_Search_Providers_For_Report_Is_Called_With_No_Selected_Routes : IDisposable
    {
        private readonly ProviderProximityReportDto _results;
        private readonly MatchingDbContext _dbContext;
        private readonly ProviderVenue _providerVenue;

        public When_SqlSearchProvider_Search_Providers_For_Report_Is_Called_With_No_Selected_Routes()
        {
            var logger = Substitute.For<ILogger<Data.SearchProviders.SqlSearchProvider>>();

            _dbContext = new TestConfiguration().GetDbContext();

            _providerVenue = new ValidProviderVenueSearchBuilder().BuildOneVenue();
            _dbContext.Add(_providerVenue);
            _dbContext.SaveChanges();

            var provider = new Data.SearchProviders.SqlSearchProvider(logger, _dbContext);

            _results = provider.SearchProvidersByPostcodeProximityForReportAsync(
                new ProviderProximitySearchParametersDto
                {
                    Postcode = "CV1 2WT", 
                    SearchRadius = 5, 
                    SelectedRoutes = null, 
                    Latitude = "52.400997", 
                    Longitude = "-1.508122"
                }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Exactly_One_Provider_Is_Found_Within_Search_Radius()
        {
            _results.Should().NotBeNull();
            
            _results.Providers.Count.Should().Be(1);

            var item = _results.Providers.First();
            item.ProviderName.Should().Be("SQL Search Provider (CV1 2WT)");
            item.ProviderDisplayName.Should().Be("SQL Search Provider");

            item.ProviderVenuePostcode.Should().Be("CV1 2WT");
            item.ProviderVenueName.Should().Be("CV1 2WT");
            item.ProviderVenueTown.Should().Be("Coventry");
            item.Distance.Should().Be(0);
            item.PrimaryContact.Should().Be("Test");
            item.PrimaryContactEmail.Should().Be("Test@test.com");
            item.PrimaryContactPhone.Should().Be("0123456789");
            item.SecondaryContact.Should().Be("Test 2");
            item.SecondaryContactEmail.Should().Be("Test2@test.com");
            item.SecondaryContactPhone.Should().Be("0987654321");

            item.Routes.Count().Should().Be(1);
            
            var route = item.Routes.First();
            route.RouteId.Should().Be(7);
            route.RouteName.Should().Be("Education and childcare");

            route.QualificationShortTitles.Count().Should().Be(1);
            var qualification = route.QualificationShortTitles.First();

            qualification.Should().Be("Short Title");
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
