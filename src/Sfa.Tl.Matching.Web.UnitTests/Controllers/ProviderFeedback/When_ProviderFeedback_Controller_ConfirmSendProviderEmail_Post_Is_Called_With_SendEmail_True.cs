using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderFeedback
{
    public class When_ProviderFeedback_Controller_ConfirmSendProviderEmail_Post_Is_Called_With_SendEmail_True
    {
        private readonly IActionResult _result;
        private readonly IProviderFeedbackService _providerFeedbackService;

        public When_ProviderFeedback_Controller_ConfirmSendProviderEmail_Post_Is_Called_With_SendEmail_True()
        {
            var providerService = Substitute.For<IProviderService>();
            _providerFeedbackService = Substitute.For<IProviderFeedbackService>();

            const string adminEmail = "admin@admin.com";
            var configuration = new MatchingConfiguration
            {
                AuthorisedAdminUserEmail = adminEmail
            };

            var providerFeedbackController = new ProviderFeedbackController(_providerFeedbackService, providerService, configuration)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(
                            new ClaimsIdentity(
                                new List<Claim>
                                {
                                    new Claim(ClaimTypes.Role, RolesExtensions.AdminUser),
                                    new Claim(ClaimTypes.Upn, adminEmail)
                                }))
                    }
                }
            };

            var viewModel = new ConfirmSendProviderEmailViewModel
            {
                ProviderCount = 100,
                SendEmail = true
            };
            _result = providerFeedbackController.ConfirmSendProviderEmail(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderFeedbackService_SendProviderQuarterlyUpdateEmailAsync_Is_Called_Exactly_Once()
        {
            _providerFeedbackService
                .Received(1)
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