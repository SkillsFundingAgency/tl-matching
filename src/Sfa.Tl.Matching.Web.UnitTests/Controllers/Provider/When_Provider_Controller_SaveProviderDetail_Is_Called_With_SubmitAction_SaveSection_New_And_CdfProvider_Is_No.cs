using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_New_And_CdfProvider_Is_No
    {
        private readonly IActionResult _result;

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveSection_New_And_CdfProvider_Is_No()
        {
            var providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(providerService, new MatchingConfiguration());
            var controllerWithClaims = new ClaimsBuilder<ProviderController>(providerController).Build();

            _result = controllerWithClaims.SaveProviderDetail(new ProviderDetailViewModel
            {
                Id = 1,
                SubmitAction = "SaveSection",
                IsCdfProvider = false
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null()
        {
            _result.Should().NotBeNull();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Provider_Detail_With_Provider_Id()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo("GetProviderDetail");
            redirect?.RouteValues
                .Should()
                .Contain(new KeyValuePair<string, object>("providerId", 1));
        }
    }
}