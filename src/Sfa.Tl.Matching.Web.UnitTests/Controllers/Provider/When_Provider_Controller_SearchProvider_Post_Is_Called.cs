using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SearchProvider_Post_Is_Called
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;

        public When_Provider_Controller_SearchProvider_Post_Is_Called()
        {
            _providerService = Substitute.For<IProviderService>();
            _providerService
                .SearchAsync(Arg.Any<long>())
                .Returns(new ProviderSearchResultDto
                    {
                        Id = 1,
                        UkPrn = 10000546,
                        Name = "Test Provider"
                    });

            var providerController = new ProviderController(_providerService, new MatchingConfiguration())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))
                    }
                }
            };

            var viewModel = new ProviderSearchParametersViewModel { UkPrn = 10000546 };
            _result = providerController.SearchProvider(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderService_SearchAsync_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .SearchAsync(Arg.Any<long>());
        }

        [Fact]
        public void Then_ProviderService_SearchProvidersWithFundingAsync_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .SearchProvidersWithFundingAsync(Arg.Any<ProviderSearchParametersViewModel>());
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Result_Is_ViewResult() =>
            _result.Should().BeOfType<ViewResult>();

        [Fact]
        public void Then_Result_Is_Redirect_To_Provider_Detail()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo("GetProviderDetail");
        }
        
        [Fact]
        public void Then_Result_Is_Redirect_With_Id()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteValues
                .Should()
                .Contain(new KeyValuePair<string, object>("providerId", 1));
        }
    }
}