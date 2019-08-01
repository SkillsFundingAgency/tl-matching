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
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Get_Provider_Detail_By_Id_With_Venues
    {
        private readonly ProviderDetailViewModel _result;

        public When_ProviderService_Is_Called_To_Get_Provider_Detail_By_Id_With_Venues()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);

            var logger = Substitute.For<ILogger<ProviderRepository>>();
            var providerReferenceRepository = Substitute.For<IRepository<Domain.Models.ProviderReference>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidProviderBuilder()
                    .AddProviderVenuesWithQualifications()
                    .Build());
                dbContext.SaveChanges();

                var providerRepository = new ProviderRepository(logger, dbContext);

                var service = new ProviderService(mapper, providerRepository, providerReferenceRepository);

                _result = service.GetProviderDetailByIdAsync(1).GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_The_Provider_Data_Is_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.UkPrn.Should().Be(10000546);
            _result.Name.Should().Be("Test Provider");
            _result.DisplayName.Should().Be("Test Provider Display Name");
            _result.PrimaryContact.Should().Be("Test");
            _result.PrimaryContactEmail.Should().Be("Test@test.com");
            _result.PrimaryContactPhone.Should().Be("0123456789");
            _result.SecondaryContact.Should().Be("Test 2");
            _result.SecondaryContactEmail.Should().Be("Test2@test.com");
            _result.SecondaryContactPhone.Should().Be("0123456789");
            _result.IsEnabledForReferral.Should().Be(true);
            _result.IsCdfProvider.Should().Be(true);
        }

        [Fact]
        public void Then_The_Provider_Venue1_data_Is_As_Expected()
        {
            _result.ProviderVenues.Count.Should().Be(2);
            _result.ProviderVenues.ElementAt(0).Postcode.Should().Be("CV1 1WT");
            _result.ProviderVenues.ElementAt(0).ProviderVenueId.Should().Be(10);
            _result.ProviderVenues.ElementAt(0).IsEnabledForReferral.Should().Be(true);
            _result.ProviderVenues.ElementAt(0).IsRemoved.Should().Be(false);
        }

        [Fact]
        public void Then_The_Provider_Venue1_Has_One_Qualification()
        {
            _result.ProviderVenues.ElementAt(0).QualificationCount.Should().Be(1);
        }

        [Fact]
        public void Then_The_Provider_Venue2_data_Is_As_Expected()
        {
            _result.ProviderVenues.Count.Should().Be(2);
            _result.ProviderVenues.ElementAt(1).Postcode.Should().Be("CV1 2WT");
            _result.ProviderVenues.ElementAt(1).ProviderVenueId.Should().Be(20);
            _result.ProviderVenues.ElementAt(1).IsEnabledForReferral.Should().Be(true);
            _result.ProviderVenues.ElementAt(1).IsRemoved.Should().Be(false);
        }

        [Fact]
        public void Then_The_Provider_Venue2_Has_Two_Qualification()
        {
            _result.ProviderVenues.ElementAt(1).QualificationCount.Should().Be(2);
        }
    }
}