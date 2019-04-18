using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Add_Is_Loaded
    {
        private readonly IActionResult _result;
        private const long UkPrn = 123456;
        private const int ProviderId = 1;

        public When_ProviderVenue_Add_Is_Loaded()
        {
            var providerService = Substitute.For<IProviderService>();
            providerService.GetProviderByUkPrnAsync(UkPrn).Returns(new ProviderDto
            {
                Id = ProviderId
            });

            var providerVenueService = Substitute.For<IProviderVenueService>();

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);

            var providerVenueController = new ProviderVenueController(mapper,
                providerService,
                providerVenueService);

            _result = providerVenueController.AddProviderVenue(UkPrn).GetAwaiter().GetResult();
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
        public void Then_UkPrn_Is_Set()
        {
            var viewModel = _result.GetViewModel<AddProviderVenueViewModel>();
            viewModel.UkPrn.Should().Be(UkPrn);
        }

        [Fact]
        public void Then_ProviderId_Is_Set()
        {
            var viewModel = _result.GetViewModel<AddProviderVenueViewModel>();
            viewModel.ProviderId.Should().Be(ProviderId);
        }
    }
}