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
    public class When_ProviderVenueService_Is_Called_To_CreateVenue
    {
        private const string UnFormatedPostcode = "CV12WT";
        private const string FormatedPostcode = "CV1 2WT";

        private readonly IProviderVenueRepository _providerVenueRepository;
        private readonly ILocationApiClient _locationApiClient;
        private readonly IGoogleMapApiClient _googleMapApiClient;

        public When_ProviderVenueService_Is_Called_To_CreateVenue()
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

            _providerVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new Domain.Models.ProviderVenue());

            _locationApiClient = Substitute.For<ILocationApiClient>();
            _locationApiClient.GetGeoLocationData(UnFormatedPostcode).Returns(new PostCodeLookupResultDto
            {
                Postcode = FormatedPostcode,
                Longitude = "1.2",
                Latitude = "1.2"
            });

            _googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            _googleMapApiClient.GetAddressDetails(Arg.Is<string>(s => s == FormatedPostcode)).Returns("Coventry");

            var providerVenueService = new ProviderVenueService(mapper, _providerVenueRepository, _locationApiClient, _googleMapApiClient);

            var viewModel = new AddProviderVenueViewModel
            {
                Postcode = UnFormatedPostcode
            };

            providerVenueService.CreateVenueAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_LocationApiClient_GetGeoLocationData_Is_Called_Exactly_Once()
        {
            _locationApiClient.Received(1).GetGeoLocationData(UnFormatedPostcode);
        }

        [Fact]
        public void Then_GoogleMapApiClient_GetAddressDetails_Is_Called_Exactly_Once()
        {
            _googleMapApiClient.Received(1).GetAddressDetails(FormatedPostcode);
        }

        [Fact]
        public void Then_ProviderVenueRepository_Create_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).Create(Arg.Is<Domain.Models.ProviderVenue>(venue =>
                venue.Postcode == FormatedPostcode &&
                venue.Town == "Coventry" &&
                venue.Name == FormatedPostcode &&
                venue.Longitude == 1.2m &&
                venue.Latitude == 1.2m));
        }
    }
}