﻿using System;
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
    public class When_SqlSearchProvider_Search_Is_Called_With_Valid_Postcode_But_No_Provider_For_Selected_Route : IDisposable
    {
        private readonly IEnumerable<SearchResultsViewModelItem> _results;
        private readonly MatchingDbContext _dbContext;
        private readonly ProviderVenue _providerVenue;

        public When_SqlSearchProvider_Search_Is_Called_With_Valid_Postcode_But_No_Provider_For_Selected_Route()
        {
            var logger = Substitute.For<ILogger<Data.SearchProviders.SqlSearchProvider>>();

            _dbContext = new TestConfiguration().GetDbContext();

            _providerVenue = new ValidProviderVenueSearchBuilder().BuildOneVenue();
            _dbContext.Add(_providerVenue);
            _dbContext.SaveChanges();

            var provider = new Data.SearchProviders.SqlSearchProvider(logger, _dbContext);

            _results = provider.SearchProvidersByPostcodeProximityAsync(new ProviderSearchParametersDto { Postcode = "MK1 1AD", SearchRadius = 5, SelectedRouteId = 1, Latitude = "52.010709", Longitude = "-0.736412" }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_Provider_Is_Found_Within_Search_Radius()
        {
            _results.Should().NotBeNull();
            _results.Count().Should().Be(0);
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