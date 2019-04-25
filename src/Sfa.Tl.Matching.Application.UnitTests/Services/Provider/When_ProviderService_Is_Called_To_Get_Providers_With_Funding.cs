using System.Collections.Generic;
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
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Get_Providers_With_Funding
    {
        private readonly IList<ProviderWithFundingDto> _result;
        private readonly IRepository<Domain.Models.Provider> _providerRepository;

        public When_ProviderService_Is_Called_To_Get_Providers_With_Funding()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);
            
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Provider>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidProviderListBuilder().Build());
                dbContext.SaveChanges();

                _providerRepository = new GenericRepository<Domain.Models.Provider>(logger, dbContext);

                var service = new ProviderService(mapper, _providerRepository);

                _result = service.GetProvidersWithFundingAsync().GetAwaiter().GetResult();
            }
        }
        
        [Fact]
        public void Then_One_Provider_Should_Be_Returned()
        {
            _result.Count.Should().Be(1);
        }

        [Fact]
        public void Then_The_Provider_Fields_Are_As_Expected()
        {
            var provider = _result.First();
            provider.Id.Should().Be(1);
            provider.Name.Should().Be("ProviderName");
            provider.PrimaryContact.Should().Be("PrimaryContact");
            provider.PrimaryContactEmail.Should().Be("primary@contact.co.uk");
            provider.PrimaryContactPhone.Should().Be("01777757777");
            provider.SecondaryContact.Should().Be("SecondaryContact");
            provider.SecondaryContactEmail.Should().Be("secondary@contact.co.uk");
            provider.SecondaryContactPhone.Should().Be("01777757777");
        }
    }
}
