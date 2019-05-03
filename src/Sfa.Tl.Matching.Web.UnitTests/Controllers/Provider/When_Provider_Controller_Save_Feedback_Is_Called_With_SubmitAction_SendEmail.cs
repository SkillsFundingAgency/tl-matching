using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_Save_Feedback_Is_Called_With_SubmitAction_SendEmail
    {
        private readonly IProviderService _providerService;

        private readonly IActionResult _result;
        private readonly SaveProviderFeedbackViewModel _viewModel;

        public When_Provider_Controller_Save_Feedback_Is_Called_With_SubmitAction_SendEmail()
        {
            _viewModel = new SaveProviderFeedbackViewModel
            {
                Providers = new List<ProviderSearchResultItemViewModel>(),
                SubmitAction = "SendEmail"
            };

            _providerService = Substitute.For<IProviderService>();
            var matchingConfiguration = Substitute.For<MatchingConfiguration>();

            var providerController = new ProviderController(_providerService, matchingConfiguration);

            _result = providerController.SaveProviderFeedback(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateProviderFeedback_Is_Called_Exactly_Once()
        {
            _providerService.Received(1).UpdateProvider(_viewModel);
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_SearchProvider()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("ConfirmSendProviderEmail");
        }
    }
}