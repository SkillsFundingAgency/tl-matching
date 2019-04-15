using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Get_Provider_Details_By_ProviderId
    {
        private const int UkPrn = 1;

        private readonly ProviderDetailViewModel _result;
        private readonly IRepository<Domain.Models.Provider> _providerRepository;

        public When_ProviderService_Is_Called_To_Get_Provider_Details_By_ProviderId()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);
            _providerRepository = Substitute.For<IRepository<Domain.Models.Provider>>();

            _providerRepository.GetMany(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>())
                .Returns(new List<Domain.Models.Provider>
                {
                    new ValidProviderBuilder().BuildWithVenueAndQualifications()
                }.AsQueryable());

            var service = new ProviderService(mapper, _providerRepository);

            _result = service.GetByIdAsync(UkPrn).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerRepository.Received(1).GetMany(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>());
        }
        
        [Fact]
        public void Then_The_Provider_Data_Is_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.UkPrn.Should().Be(10000546);
            _result.Name.Should().Be("Test Name");
            _result.DisplayName.Should().Be("Test DisplayName");
            _result.PrimaryContact.Should().Be("Test");
            _result.PrimaryContactEmail.Should().Be("Test@test.com");
            _result.PrimaryContactPhone.Should().Be("0123456789");
            _result.SecondaryContact.Should().Be("Test 2");
            _result.SecondaryContactEmail.Should().Be("Test2@test.com");
            _result.SecondaryContactPhone.Should().Be("0123456789");
            _result.IsEnabledForReferral.Should().Be(true);
            _result.IsEnabledForSearch.Should().Be(true);
        }

        [Fact]
        public void Then_The_Provider_Venue1_data_Is_As_Expected()
        {
            _result.ProviderVenues.Count.Should().Be(2);
            _result.ProviderVenues.ElementAt(0).Postcode.Should().Be("CV1 1WT");
            _result.ProviderVenues.ElementAt(0).ProviderVenueId.Should().Be(10);
            _result.ProviderVenues.ElementAt(0).IsEnabledForSearch.Should().Be(true);
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
            _result.ProviderVenues.ElementAt(1).IsEnabledForSearch.Should().Be(true);
        }

        [Fact]
        public void Then_The_Provider_Venue2_Has_Two_Qualification()
        {
            _result.ProviderVenues.ElementAt(1).QualificationCount.Should().Be(2);
        }
   }
}
