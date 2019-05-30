using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue
{
    public class When_ProviderVenueService_Is_Called_To_HaveUniqueVenue_Exists
    {
        private readonly IRepository<Domain.Models.ProviderVenue> _providerVenueRepository;
        private readonly ProviderVenueDetailViewModel _result;
        private const int ProviderId = 1;
        private const string Name = "Name";
        private const string Postcode = "CV1 2WT";

        public When_ProviderVenueService_Is_Called_To_HaveUniqueVenue_Exists()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(ProviderVenueMapper).Assembly));
            var mapper = new Mapper(config);
            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();

            _providerVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new Domain.Models.ProviderVenue
                {
                    ProviderId = ProviderId,
                    Name = Name,
                    Postcode = Postcode,
                });

            var locationService = Substitute.For<ILocationService>();
            var providerVenueService = new ProviderVenueService(mapper, _providerVenueRepository,
                locationService);

            _result = providerVenueService.GetVenue(ProviderId, Postcode).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>());
        }

        [Fact]
        public void Then_The_ProviderId_Is_Correct()
        {
            _result.ProviderId.Should().Be(ProviderId);
        }

        [Fact]
        public void Then_The_Name_Is_Correct()
        {
            _result.Name.Should().Be(Name);
        }
    }
}