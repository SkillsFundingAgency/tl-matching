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
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_Existing_And_CdfProvider_Is_Yes
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;
        private readonly ProviderDetailViewModel _viewModel = new()
        {
            Id = 1,
            SubmitAction = "SaveSection",
            IsCdfProvider = true
        };

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_Existing_And_CdfProvider_Is_Yes()
        {
            _providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(_providerService, new MatchingConfiguration());
            var controllerWithClaims = new ClaimsBuilder<ProviderController>(providerController).Build();

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
            _providerService.Received(1).UpdateProviderDetailSectionAsync(Arg.Is(_viewModel));
        }
    }
}