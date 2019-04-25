using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Get_Providers_With_Funding_Count
    {
        private readonly int _result;

        public When_ProviderService_Is_Called_To_Get_Providers_With_Funding_Count()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);

            var logger = Substitute.For<ILogger<ProviderRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidProviderListBuilder().Build());
                dbContext.SaveChanges();

                var providerRepository = new ProviderRepository(logger, dbContext);

                var service = new ProviderService(mapper, providerRepository);

                _result = service.GetProvidersWithFundingCountAsync().GetAwaiter().GetResult();
            }
        }
        
        [Fact]
        public void Then_The_Provider_Count_Is_As_Expected()
        {
            _result.Should().Be(1);
        }
    }
}
