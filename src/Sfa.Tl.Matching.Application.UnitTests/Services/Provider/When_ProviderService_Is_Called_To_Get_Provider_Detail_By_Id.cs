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
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Get_Provider_Detail_By_Id
    {
        private readonly ProviderDetailViewModel _result;

        public When_ProviderService_Is_Called_To_Get_Provider_Detail_By_Id()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);

            var logger = Substitute.For<ILogger<ProviderRepository>>();
            var providerReferenceRepository = Substitute.For<IRepository<ProviderReference>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidProviderBuilder()
                    .Build());
                dbContext.SaveChanges();

                var providerRepository = new ProviderRepository(logger, dbContext);

                var service = new ProviderService(mapper, providerRepository, providerReferenceRepository);

                _result = service.GetProviderDetailByIdAsync(1).GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_The_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.UkPrn.Should().Be(10000546);
            _result.Name.Should().Be("Test Provider");
            _result.DisplayName.Should().Be("Test Provider Display Name");
            _result.IsEnabledForReferral.Should().Be(true);
            _result.IsCdfProvider.Should().Be(true);
            _result.PrimaryContact.Should().Be("Test");
            _result.PrimaryContactEmail.Should().Be("Test@test.com");
            _result.PrimaryContactPhone.Should().Be("0123456789");
            _result.SecondaryContact.Should().Be("Test 2");
            _result.SecondaryContactEmail.Should().Be("Test2@test.com");
            _result.SecondaryContactPhone.Should().Be("0123456789");
            _result.ProviderVenues.Should().NotBeNull();
            _result.ProviderVenues.Count.Should().Be(0);
        }
    }
}