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
    public class When_HomeController_SaveServiceStatusHistory_Is_Called_By_Standard_User
    {
        private readonly IActionResult _result;

        private readonly IServiceStatusHistoryService _serviceStatusHistoryService;

        public When_HomeController_SaveServiceStatusHistory_Is_Called_By_Standard_User()
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
            
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);
          
            var viewModel = new ServiceStatusHistoryViewModel
            {
                IsOnline = true
            };

            _result = homeController.SaveServiceStatusHistoryAsync(viewModel).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_SaveServiceStatusHistoryAsync_Is_Not_Called()
        {
            _serviceStatusHistoryService
                .DidNotReceive()
                .SaveServiceStatusHistoryAsync(
                    Arg.Any<ServiceStatusHistoryViewModel>());
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