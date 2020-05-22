using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Home
{
    public class When_HomeController_Maintenance_Is_Loaded_By_Admin_User
    {
        private readonly IActionResult _result;

        private readonly IServiceStatusHistoryService _serviceStatusHistoryService;

        public When_HomeController_Maintenance_Is_Loaded_By_Admin_User()
        {
            _serviceStatusHistoryService = Substitute.For<IServiceStatusHistoryService>();
            _serviceStatusHistoryService.GetLatestServiceStatusHistoryAsync()
                .Returns(new ServiceStatusHistoryViewModel
            {
                IsOnline = true
            });

            var homeController = new HomeController(_serviceStatusHistoryService);

            var controllerWithClaims = new ClaimsBuilder<HomeController>(homeController)
                .AddAdminUser()
                .Build();

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = homeController.MaintenanceAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ServiceStatusHistoryViewModel_Is_Populated_Correctly()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();

            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<ServiceStatusHistoryViewModel>();

            viewModel.IsOnline.Should().BeTrue();
            viewModel.StatusDisplayText.Should().Be("Take offline");
            viewModel.BodyDisplayText.Should().Be("online");
        }

        [Fact]
        public void Then_GetLatestServiceStatusHistoryAsync_Is_Called_Exactly_Once()
        {
            _serviceStatusHistoryService
                .Received(1)
                .GetLatestServiceStatusHistoryAsync();
        }
    }
}