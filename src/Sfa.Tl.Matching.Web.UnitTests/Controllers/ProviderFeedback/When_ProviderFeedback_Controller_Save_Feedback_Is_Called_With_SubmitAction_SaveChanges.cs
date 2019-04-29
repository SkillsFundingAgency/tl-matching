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
    public class When_ProviderFeedback_Controller_Save_Feedback_Is_Called_With_SubmitAction_SaveChanges
    {
        private readonly IProviderFeedbackService _providerFeedbackService;

        private readonly IActionResult _result;
        private readonly SaveProviderFeedbackViewModel _viewModel;

        public When_ProviderFeedback_Controller_Save_Feedback_Is_Called_With_SubmitAction_SaveChanges()
        {
            _viewModel = new SaveProviderFeedbackViewModel
            {
                Providers = new List<ProviderSearchResultItemViewModel>(),
                SubmitAction = "SaveChanges"
            };
            _providerFeedbackService = Substitute.For<IProviderFeedbackService>();
            var matchingConfiguration = Substitute.For<MatchingConfiguration>();

            var providerFeedbackController = new ProviderFeedbackController(_providerFeedbackService, matchingConfiguration);

            _result = providerFeedbackController.SaveProviderFeedback(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateProviderFeedback_Is_Called_Exactly_Once()
        {
            _providerFeedbackService.Received(1).UpdateProviderFeedback(_viewModel);
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_SearchProvider()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("SearchProvider");
        }
    }
}