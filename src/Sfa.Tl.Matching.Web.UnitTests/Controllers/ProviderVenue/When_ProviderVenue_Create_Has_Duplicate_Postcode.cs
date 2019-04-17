using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Create_Postcode_Exists
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;
        private const long UkPrn = 123456;
        private const string Postcode = "CV1 2WT";

        public When_ProviderVenue_Create_Postcode_Exists()
        {
            var providerService = Substitute.For<IProviderService>();

            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.IsValidPostCodeAsync(Postcode).Returns((true, Postcode));
            _providerVenueService.HaveUniqueVenueAsync(UkPrn, Postcode).Returns(false);

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderVenueDtoMapper).Assembly));
            var mapper = new Mapper(config);

            var providerVenueController = new ProviderVenueController(mapper,
                providerService,
                _providerVenueService);

            var viewModel = new ProviderVenueAddViewModel
            {
                UkPrn = UkPrn,
                Postcode = Postcode
            };

            _result = providerVenueController.ProviderVenueAdd(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_RedirectToRoute_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<RedirectToRouteResult>();

        [Fact]
        public void Then_Result_Is_RedirectToRoute()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();
            result?.RouteName.Should().Be("GetProviderVenueDetail");
        }

        [Fact]
        public void Then_IsValidPostCode_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).IsValidPostCodeAsync(Postcode);
        }

        [Fact]
        public void Then_HaveUniqueVenue_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).HaveUniqueVenueAsync(UkPrn, Postcode);
        }

        [Fact]
        public void Then_CreateVenue_Is_Not_Called()
        {
            _providerVenueService.DidNotReceive().CreateVenueAsync(Arg.Any<ProviderVenueDto>());
        }
    }
}