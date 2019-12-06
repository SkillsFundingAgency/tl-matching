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

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderQuarterlyUpdateEmail
{
    public class When_ProviderQuarterlyUpdateEmail_Controller_ConfirmSendProviderEmail_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;

        public When_ProviderQuarterlyUpdateEmail_Controller_ConfirmSendProviderEmail_Is_Loaded()
        {
            _providerService = Substitute.For<IProviderService>();
            var providerQuarterlyUpdateEmailService = Substitute.For<IProviderQuarterlyUpdateEmailService>();

            _providerService
                .GetProvidersWithFundingCountAsync()
                .Returns(42);

            const string adminEmail = "admin@admin.com";
            var configuration = new MatchingConfiguration
            {
                AuthorisedAdminUserEmail = adminEmail
            };

            var providerQuarterlyUpdateEmailController =
                new ProviderQuarterlyUpdateEmailController(providerQuarterlyUpdateEmailService, _providerService, configuration);
            var controllerWithClaims = new ClaimsBuilder<ProviderQuarterlyUpdateEmailController>(providerQuarterlyUpdateEmailController)
                .Add(ClaimTypes.Role, RolesExtensions.AdminUser)
                .AddEmail(adminEmail)
                .Build();

            _result = controllerWithClaims.ConfirmSendProviderEmailAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderService_GetProvidersWithFundingCountAsync_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .GetProvidersWithFundingCountAsync();
        }
        
        [Fact]
        public void Then_Model_Has_Expected_Values()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            viewResult?.Model.Should().BeOfType<ConfirmSendProviderEmailViewModel>();
            
            var model = viewResult?.Model as ConfirmSendProviderEmailViewModel;
            model.Should().NotBeNull();
            model?.ProviderCount.Should().Be(42);
            model?.SendEmail.Should().Be(null);
        }
    }
}