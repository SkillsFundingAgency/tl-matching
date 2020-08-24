using System;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue
{
    public class When_ProviderVenueService_Is_Called_To_Create_Venue
    {
        private const string UnFormattedPostcode = "CV12WT";
        private const string FormattedPostcode = "CV1 2WT";

        private readonly IProviderVenueRepository _providerVenueRepository;
        private readonly ILocationApiClient _locationApiClient;
        private readonly IGoogleMapApiClient _googleMapApiClient;

        public When_ProviderVenueService_Is_Called_To_Create_Venue()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ProviderVenueMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<AddProviderVenueViewModel, Domain.Models.ProviderVenue>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<AddProviderVenueViewModel, Domain.Models.ProviderVenue>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<AddProviderVenueViewModel, Domain.Models.ProviderVenue>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);
            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();

            _providerVenueRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new Domain.Models.ProviderVenue());

            _locationApiClient = Substitute.For<ILocationApiClient>();
            _locationApiClient.GetGeoLocationDataAsync(UnFormattedPostcode, true).Returns(new PostcodeLookupResultDto
            {
                Postcode = FormattedPostcode,
                Longitude = "1.2",
                Latitude = "1.2"
            });

            _googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            _googleMapApiClient.GetAddressDetailsAsync(Arg.Is<string>(s => s == FormattedPostcode)).Returns("Coventry");

            var providerVenueService = new ProviderVenueService(mapper, _providerVenueRepository, _locationApiClient, _googleMapApiClient);

            var viewModel = new AddProviderVenueViewModel
            {
                Postcode = UnFormattedPostcode
            };

            providerVenueService.CreateVenueAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_LocationApiClient_GetGeoLocationData_Is_Called_Exactly_Once()
        {
            _locationApiClient
                .Received(1)
                .GetGeoLocationDataAsync(UnFormattedPostcode, true);
        }

        [Fact]
        public void Then_GoogleMapApiClient_GetAddressDetails_Is_Called_Exactly_Once()
        {
            _googleMapApiClient.Received(1).GetAddressDetailsAsync(FormattedPostcode);
        }

        [Fact]
        public void Then_ProviderVenueRepository_Create_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).CreateAsync(Arg.Is<Domain.Models.ProviderVenue>(venue =>
                venue.Postcode == FormattedPostcode &&
                venue.Town == "Coventry" &&
                venue.Name == FormattedPostcode &&
                venue.IsEnabledForReferral &&
                venue.IsRemoved == false && 
                venue.Longitude == 1.2m &&
                venue.Latitude == 1.2m));
        }
    }
}