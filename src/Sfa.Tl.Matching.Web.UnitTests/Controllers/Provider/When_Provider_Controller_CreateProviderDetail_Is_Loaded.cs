using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_CreateProviderDetail_Is_Loaded
    {
        private readonly IActionResult _result;

        public When_Provider_Controller_CreateProviderDetail_Is_Loaded()
        {
            var providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(providerService, new MatchingConfiguration());
            var controllerWithClaims = new ClaimsBuilder<ProviderController>(providerController).Build();

            _result = controllerWithClaims.AddProviderDetail(new AddProviderViewModel
            {
                UkPrn = 123,
                Name = "Provider name"
            });
        }

        [Fact]
        public void Then_View_Model_Is_Correct()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            viewResult?.Model.Should().BeOfType<ProviderDetailViewModel>();

            var viewModel = _result.GetViewModel<ProviderDetailViewModel>();
            viewModel.Name.Should().Be("Provider name");
            viewModel.DisplayName.Should().Be("Provider Name");
            viewModel.UkPrn.Should().Be(123);
            viewModel.IsCdfProvider.Should().BeTrue();
            viewModel.IsEnabledForReferral.Should().BeTrue();
        }
    }
}