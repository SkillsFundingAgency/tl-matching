using System;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue
{
    public class When_ProviderVenueService_Is_Called_To_UpdateVenue_With_Postcode_For_Name
    {
        private readonly ILocationApiClient _locationApiClient;
        private readonly IProviderVenueRepository _providerVenueRepository;

        public When_ProviderVenueService_Is_Called_To_UpdateVenue_With_Postcode_For_Name()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "TestUser")
                }))
            });

            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider.UtcNow().Returns(new DateTime(2019, 1, 1));

            _locationApiClient = Substitute.For<ILocationApiClient>();
            _locationApiClient.IsValidPostcodeAsync(Arg.Any<string>(), Arg.Any<bool>())
                .Returns(callinfo => callinfo.Arg<string>() == "CV1 2WT"
                    ? (true, "CV1 2WT")
                    : (false, null));

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ProviderVenueMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<ProviderVenueDetailViewModel, Domain.Models.ProviderVenue>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<ProviderVenueDetailViewModel, Domain.Models.ProviderVenue>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<ProviderVenueDetailViewModel, Domain.Models.ProviderVenue>(dateTimeProvider) :
                                type.Name.Contains("VenueNameResolver") ?
                                    (object)new VenueNameResolver(_locationApiClient) :
                                    null);
            });
            var mapper = new Mapper(config);

            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();
            _providerVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new ValidProviderVenueBuilder().Build());

            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var locationService = Substitute.For<ILocationApiClient>();

            var providerVenueService = new ProviderVenueService(mapper, _providerVenueRepository, locationService, googleMapApiClient);

            var viewModel = new ProviderVenueDetailViewModel
            {
                Id = 1,
                Postcode = "CV1 2WT",
                Name = "CV1 2WT",
                IsEnabledForReferral = true
            };

            providerVenueService.UpdateVenueAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void LocationApiClient_IsValidPostcode_Is_Called_Exactly_Once()
        {
            _locationApiClient
                .Received(1)
                .IsValidPostcodeAsync(Arg.Is<string>(s => s == "CV1 2WT"),
                    Arg.Is<bool>(b => b));
        }

        [Fact]
        public void Then_ProviderVenueRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>());
        }

        [Fact]
        public void Then_ProviderVenueRepository_Update_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).Update(Arg.Any<Domain.Models.ProviderVenue>());
        }
        
        [Fact]
        public void Then_ProviderVenueRepository_Update_Is_Called_With_Expected_Values()
        {
            _providerVenueRepository.Received(1)
                .Update(Arg.Is<Domain.Models.ProviderVenue>(
                    pv => pv.Id == 1 &&
                    pv.Postcode == "CV1 2WT" &&
                    pv.Name == "CV1 2WT" &&
                    pv.IsEnabledForReferral &&
                    !pv.IsRemoved &&
                    pv.ModifiedBy == "TestUser" &&
                    pv.ModifiedOn == new DateTime(2019, 1, 1)));
        }
    }
}