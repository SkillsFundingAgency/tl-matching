using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderFeedback
{
    public class When_ProviderFeedback_Controller_ConfirmSendProviderEmail_Post_Is_Called_With_SendEmail_False
    {
        private readonly IActionResult _result;
        private readonly IProviderFeedbackService _providerFeedbackService;

        public When_ProviderFeedback_Controller_ConfirmSendProviderEmail_Post_Is_Called_With_SendEmail_False()
        {
            var providerService = Substitute.For<IProviderService>();
            _providerFeedbackService = Substitute.For<IProviderFeedbackService>();

            var providerFeedbackController = new ProviderFeedbackController(_providerFeedbackService, providerService, new MatchingConfiguration());

            var viewModel = new ConfirmSendProviderEmailViewModel
            {
                ProviderCount = 100,
                SendEmail = false
            };
            _result = providerFeedbackController.ConfirmSendProviderEmail(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderFeedbackService_SendProviderQuarterlyUpdateEmailAsync_Is_Not_Called()
        {
            _providerFeedbackService
                .DidNotReceive()
                .SendProviderQuarterlyUpdateEmailAsync();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Result_Is_RedirectResult() =>
            _result.Should().BeOfType<RedirectToRouteResult>();

        [Fact]
        public void Then_Result_Is_Redirect_To_Search_Providers()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo("SearchProvider");
        }
    }
}