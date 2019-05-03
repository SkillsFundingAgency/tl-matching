using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider
{
    public class When_SqlSearchProvider_Search_Is_Called_With_InValid_PostCode
    {
        private readonly Data.SearchProviders.SqlSearchProvider _provider;

        public When_SqlSearchProvider_Search_Is_Called_With_InValid_PostCode()
        {
            var logger = Substitute.For<ILogger<Data.SearchProviders.SqlSearchProvider>>();

            var dbContext = new TestConfiguration().GetDbContext();
            _provider = new Data.SearchProviders.SqlSearchProvider(logger, dbContext);
        }

        [Fact]
        public void Then_Results_Should_Not_Be_Null()
        {
            Action action = () => _provider.SearchProvidersByPostcodeProximity(new ProviderSearchParametersDto { Postcode = "CV1 234", SearchRadius = 5, SelectedRouteId = 7, Latitude = "", Longitude = "" }).GetAwaiter().GetResult();
            action.Should().ThrowExactly<InvalidOperationException>();
        }
    }
}