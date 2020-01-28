using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_New_And_CdfProvider_Is_Yes : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;
        private readonly ProviderControllerFixture _fixture;
        private readonly ProviderDetailViewModel _viewModel = new ProviderDetailViewModel
        {
            UkPrn = 123,
            Name = "ProviderName",
            SubmitAction = "SaveSection",
            IsCdfProvider = true
        };

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_New_And_CdfProvider_Is_Yes()
        {
            _fixture = new ProviderControllerFixture();
            
            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

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
            _fixture.ProviderService.DidNotReceive().UpdateProviderDetailSectionAsync(Arg.Is(_viewModel));
        }
    }
}