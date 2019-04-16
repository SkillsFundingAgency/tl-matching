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
    public class When_ProviderVenue_Create_Has_Invalid_Postcode
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;
        private const long UkPrn = 123456;
        private const int ProviderId = 1;
        private const string Postcode = "CV1 2WT";

        public When_ProviderVenue_Create_Has_Invalid_Postcode()
        {
            var providerService = Substitute.For<IProviderService>();
            providerService.GetProviderByUkPrnAsync(UkPrn).Returns(new ProviderDto
            {
                Id = ProviderId
            });

            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.IsValidPostCode(Postcode).Returns((false, Postcode));

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderVenueDtoMapper).Assembly));
            var mapper = new Mapper(config);

            var providerVenueController = new ProviderVenueController(mapper,
                providerService,
                _providerVenueService);

            var viewModel = new ProviderVenueAddViewModel
            {
                Postcode = Postcode
            };

            _result = providerVenueController.ProviderVenueAdd(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_IsValidPostCode_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).IsValidPostCode(Postcode);
        }

        [Fact]
        public void Then_CreateVenue_Is_Not_Called()
        {
            _providerVenueService.DidNotReceive().CreateVenue(Arg.Any<ProviderVenueDto>());
        }
    }
}