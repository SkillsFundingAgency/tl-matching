using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_AddProvider_Post_Is_Called
    {
        private readonly IActionResult _result;

        public When_Provider_Controller_AddProvider_Post_Is_Called()
        {
            var providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(providerService, new MatchingConfiguration());
            var controllerWithClaims = new ClaimsBuilder<ProviderController>(providerController).Build();

            var viewModel = new AddProviderViewModel
            {
                UkPrn = 123,
                Name = "ProviderName"
            };
            _result = controllerWithClaims.AddProvider(viewModel);
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
        public void Then_Result_Is_RedirectResult() =>
            _result.Should().BeOfType<RedirectToRouteResult>();

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo("CreateProviderDetail");
        }

        [Fact]
        public void Then_Result_Contains_Correct_Results()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteValues["ukPrn"].Should().Be(123);
            redirect?.RouteValues["name"].Should().Be("ProviderName");
        }
    }
}