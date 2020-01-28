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
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_Existing_And_CdfProvider_Is_Yes : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;
        private readonly ProviderControllerFixture _fixture;

        private readonly ProviderDetailViewModel _viewModel = new ProviderDetailViewModel
        {
            Id = 1,
            SubmitAction = "SaveSection",
            IsCdfProvider = true
        };

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_Existing_And_CdfProvider_Is_Yes()
        {
            _fixture = new ProviderControllerFixture();
            
            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            _result = controllerWithClaims.SaveProviderDetailAsync(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Add_Provider_Detail_With_Provider_Id()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<RedirectToRouteResult>();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();

            redirect?.RouteName.Should().BeEquivalentTo("GetProviderDetail");
            redirect?.RouteValues
                .Should()
                .Contain(new KeyValuePair<string, object>("providerId", 1));
        }

        [Fact]
        public void Then_ProviderService_UpdateProviderDetailSectionAsync_Called()
        {
            _fixture.ProviderService.Received(1).UpdateProviderDetailSectionAsync(Arg.Is(_viewModel));
        }
    }
}