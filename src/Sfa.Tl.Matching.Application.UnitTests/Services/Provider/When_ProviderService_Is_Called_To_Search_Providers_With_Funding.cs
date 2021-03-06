﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Search_Providers_With_Funding
    {
        private readonly IList<ProviderSearchResultItemViewModel> _result;

        public When_ProviderService_Is_Called_To_Search_Providers_With_Funding()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);

            var logger = Substitute.For<ILogger<ProviderRepository>>();
            var providerReferenceRepository = Substitute.For<IRepository<ProviderReference>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.AddRange(new ValidProviderListBuilder().Build());
            dbContext.SaveChanges();

            var providerRepository = new ProviderRepository(logger, dbContext);

            var service = new ProviderService(mapper, providerRepository, providerReferenceRepository);

            _result = service.SearchProvidersWithFundingAsync(new ProviderSearchParametersViewModel()).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Two_Providers_Should_Be_Returned()
        {
            _result.Count.Should().Be(2);
        }

        [Fact]
        public void Then_The_First_Provider_Fields_Are_As_Expected()
        {
            var provider = _result.First();
            provider.Id.Should().Be(1);
            provider.UkPrn.Should().Be(10000546);
            provider.Name.Should().Be("ProviderName");
            provider.IsCdfProvider.Should().Be("Yes");
        }

        [Fact]
        public void Then_The_Second_Provider_Fields_Are_As_Expected()
        {
            var provider = _result.Skip(1).First();
            provider.Id.Should().Be(2);
            provider.UkPrn.Should().Be(10000123);
            provider.Name.Should().Be("ProviderName2");
            provider.IsCdfProvider.Should().Be("No");
        }
    }
}
