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
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_New_And_CdfProvider_Is_Yes
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;
        private readonly ProviderDetailViewModel _viewModel = new()
        {
            UkPrn = 123,
            Name = "ProviderName",
            SubmitAction = "SaveSection",
            IsCdfProvider = true
        };

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_New_And_CdfProvider_Is_Yes()
        {
            _providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(_providerService, new MatchingConfiguration());
            var controllerWithClaims = new ClaimsBuilder<ProviderController>(providerController).Build();

            _result = controllerWithClaims.SaveProviderDetailAsync(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Add_Provider_Detail_With_Correct_Route_Values()
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

        [Fact]
        public void Then_ProviderService_UpdateProviderDetailSectionAsync_Is_Not_Called()
        {
            _providerService.DidNotReceive().UpdateProviderDetailSectionAsync(Arg.Is(_viewModel));
        }
    }
}