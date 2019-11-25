using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider
{
    public class When_SqlSearchProvider_Search_Other_Routes_Is_Called_With_Invalid_Latitude_And_Longitude
    {
        private readonly Data.SearchProviders.SqlSearchProvider _provider;

        public When_SqlSearchProvider_Search_Other_Routes_Is_Called_With_Invalid_Latitude_And_Longitude()
        {
            var logger = Substitute.For<ILogger<Data.SearchProviders.SqlSearchProvider>>();

            var dbContext = new TestConfiguration().GetDbContext();
            _provider = new Data.SearchProviders.SqlSearchProvider(logger, dbContext);
        }

        [Fact]
        public void Then_Search_Should_Throw_Exception()
        {
            Action action = () => _provider.SearchOpportunitiesForOtherRoutesByPostcodeProximityAsync(new ProviderSearchParametersDto { Postcode = "CV1 2WT", SearchRadius = 5, SelectedRouteId = 7, Latitude = "", Longitude = "" }).GetAwaiter().GetResult();
            action.Should().ThrowExactly<InvalidOperationException>();
        }
    }
}