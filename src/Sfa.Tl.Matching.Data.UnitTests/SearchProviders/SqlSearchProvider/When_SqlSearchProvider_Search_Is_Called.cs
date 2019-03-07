using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.SearchProviders.SqlSearchProvider
{
    public class When_SqlSearchProvider_Search_Is_Called
    {
        private readonly IEnumerable<ProviderVenueSearchResultDto> _results;

        public When_SqlSearchProvider_Search_Is_Called()
        {
            var logger = Substitute.For<ILogger<Data.SearchProviders.SqlSearchProvider>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                //dbContext.Add(new ValidProviderVenueSearchBuilder().Build());
                //dbContext.SaveChanges();

                var provider = new Data.SearchProviders.SqlSearchProvider(logger, dbContext);

                _results = provider.SearchProvidersByPostcodeProximity(new ProviderSearchParametersDto { Postcode = "AA1 1AA", SearchRadius = 5, SelectedRouteId = 1 });
            }
        }

        [Fact]
        public void Then_Results_Should_Not_Be_Null() =>
            _results.Should().NotBeNull();

        [Fact]
        public void Then_No_Results_Are_Returned() =>
            _results.Count().Should().Be(0);
    }
}
