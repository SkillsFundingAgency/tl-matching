using System.Collections.Generic;
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
    public class When_Provider_Controller_CreateProviderDetail_Is_Called_With_SubmitAction_SaveSection_And_CdfProvider_Is_Yes
    {
        private readonly IActionResult _result;
        private readonly CreateProviderDetailViewModel _viewModel = new CreateProviderDetailViewModel
        {
            Name = "ProviderName",
            UkPrn = 123,
            SubmitAction = "SaveSection",
            IsCdfProvider = true
        };

        public When_Provider_Controller_CreateProviderDetail_Is_Called_With_SubmitAction_SaveSection_And_CdfProvider_Is_Yes()
        {
            var providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(providerService, new MatchingConfiguration());
            var controllerWithClaims = new ClaimsBuilder<ProviderController>(providerController).Build();

            _result = controllerWithClaims.CreateProviderDetailAsync(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_SearchProvider_With_Correct_Values()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<RedirectToActionResult>();
            var redirect = _result as RedirectToActionResult;
            redirect.Should().NotBeNull();
            redirect?.ActionName.Should().BeEquivalentTo("AddProviderDetail");
            redirect?.RouteValues
                .Should()
                .Contain(new KeyValuePair<string, object>("UkPrn", 123));
            redirect?.RouteValues
                .Should()
                .Contain(new KeyValuePair<string, object>("Name", "ProviderName"));
        }
    }
}