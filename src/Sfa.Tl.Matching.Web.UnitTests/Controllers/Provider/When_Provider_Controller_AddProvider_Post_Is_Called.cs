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
        public void Then_Result_Is_Redirect_To_Results()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToRouteResult>();

            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("AddProviderDetail");
            redirect?.RouteValues["ukPrn"].Should().Be(123);
            redirect?.RouteValues["name"].Should().Be("ProviderName");
        }
    }
}