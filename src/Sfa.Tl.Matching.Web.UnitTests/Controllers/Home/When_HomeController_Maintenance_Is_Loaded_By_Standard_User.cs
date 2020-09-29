using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Home
{
    public class When_HomeController_Maintenance_Is_Loaded_By_Standard_User
    {
        private readonly IActionResult _result;

        private readonly IServiceStatusHistoryService _serviceStatusHistoryService;

        public When_HomeController_Maintenance_Is_Loaded_By_Standard_User()
        {
            _serviceStatusHistoryService = Substitute.For<IServiceStatusHistoryService>();
            _serviceStatusHistoryService.GetLatestServiceStatusHistoryAsync()
                .Returns(new ServiceStatusHistoryViewModel
            {
                IsOnline = true
            });

            var homeController = new HomeController(_serviceStatusHistoryService);

            var controllerWithClaims = new ClaimsBuilder<HomeController>(homeController)
                .AddStandardUser()
                .Build();

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = homeController.MaintenanceAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetLatestServiceStatusHistoryAsync_Is_Not_Called()
        {
            _serviceStatusHistoryService
                .DidNotReceive()
                .GetLatestServiceStatusHistoryAsync();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_No_Permission()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<RedirectToRouteResult>();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("FailedLogin");
            redirect?.RouteValues
                .Should()
                .BeNull();
        }
    }
}