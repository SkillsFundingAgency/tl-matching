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
    public class When_ProviderFeedback_Controller_ConfirmSendProviderEmail_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;

        public When_ProviderFeedback_Controller_ConfirmSendProviderEmail_Is_Loaded()
        {
            _providerService = Substitute.For<IProviderService>();
            var providerFeedbackService = Substitute.For<IProviderFeedbackService>();

            _providerService
                .GetProvidersWithFundingCountAsync()
                .Returns(42);

            const string adminEmail = "admin@admin.com";
            var configuration = new MatchingConfiguration
            {
                AuthorisedAdminUserEmail = adminEmail
            };

            var providerFeedbackController =
                new ProviderFeedbackController(providerFeedbackService, _providerService, configuration);
            var controllerWithClaims = new ClaimsBuilder<ProviderFeedbackController>(providerFeedbackController)
                .Add(ClaimTypes.Role, RolesExtensions.AdminUser)
                .AddEmail(adminEmail)
                .Build();

            _result = controllerWithClaims.ConfirmSendProviderEmail().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderService_GetProvidersWithFundingCountAsync_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .GetProvidersWithFundingCountAsync();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Model_Is_Of_Type_ConfirmSendProviderEmailViewModel()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().BeOfType<ConfirmSendProviderEmailViewModel>();
        }

        [Fact]
        public void Then_Model_ProviderCount_Has_Expected_Value()
        {
            var viewResult = _result as ViewResult;
            var model = viewResult?.Model as ConfirmSendProviderEmailViewModel;
            // ReSharper disable once PossibleNullReferenceException
            model.ProviderCount.Should().Be(42);
        }

        [Fact]
        public void Then_Model_SendEmail_Has_Expected_Value()
        {
            var viewResult = _result as ViewResult;
            var model = viewResult?.Model as ConfirmSendProviderEmailViewModel;
            // ReSharper disable once PossibleNullReferenceException
            model.SendEmail.Should().Be(null);
        }
    }
}