using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderFeedback
{
    public class When_ProviderQuarterlyUpdateEmail_Controller_ConfirmSendProviderEmail_Post_Is_Called_With_SendEmail_True
    {
        private readonly IActionResult _result;
        private readonly IProviderQuarterlyUpdateEmailService _providerFeedbackService;

        public When_ProviderQuarterlyUpdateEmail_Controller_ConfirmSendProviderEmail_Post_Is_Called_With_SendEmail_True()
        {
            var providerService = Substitute.For<IProviderService>();
            _providerFeedbackService = Substitute.For<IProviderQuarterlyUpdateEmailService>();

            const string adminEmail = "admin@admin.com";
            var configuration = new MatchingConfiguration
            {
                AuthorisedAdminUserEmail = adminEmail
            };

            var providerFeedbackController =
                new ProviderQuarterlyUpdateEmailController(_providerFeedbackService, providerService, configuration);
            var controllerWithClaims = new ClaimsBuilder<ProviderQuarterlyUpdateEmailController>(providerFeedbackController)
                .Add(ClaimTypes.Role, RolesExtensions.AdminUser)
                .AddEmail(adminEmail)
                .Build();

            var viewModel = new ConfirmSendProviderEmailViewModel
            {
                ProviderCount = 100,
                SendEmail = true
            };
            _result = controllerWithClaims.ConfirmSendProviderEmailAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderFeedbackService_RequestProviderQuarterlyUpdateAsync_Is_Called_Exactly_Once()
        {
            _providerFeedbackService
                .Received(1)
                .RequestProviderQuarterlyUpdateAsync(Arg.Any<string>());
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