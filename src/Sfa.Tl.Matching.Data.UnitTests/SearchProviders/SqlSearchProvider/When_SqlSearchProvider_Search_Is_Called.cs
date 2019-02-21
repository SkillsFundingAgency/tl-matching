using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.SearchProviders.SqlSearchProvider
{
    public class When_SqlSearchProvider_Search_Is_Called
    {
        private readonly IEnumerable<ProviderVenueSearchResult> _results;
        private readonly Exception _exception;

        public When_SqlSearchProvider_Search_Is_Called()
        {
            var logger = Substitute.For<ILogger<Data.SearchProviders.SqlSearchProvider>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                //dbContext.Add(new ValidProviderVenueSearchBuilder().Build());
                //dbContext.SaveChanges();

                //TODO: This test might need to use a real or Sqlite context
                //TODO: Remove exception handling code and test when the class is implemented 
                var provider = new Data.SearchProviders.SqlSearchProvider(logger, dbContext);
                try
                {
                    _results = provider.SearchProvidersByPostcodeProximity("AA1 1AA", 5, 1)
                        .GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    _exception = e;
                }
            }
        }

        [Fact]
        public void Then_No_Results_Are_Returned() =>
            _results.Should().BeNull();

        [Fact]
        public void Then_A_Not_Implemented_Exception_Is_Raised() =>
            _exception.Should().BeOfType<NotImplementedException>();
    }
}
