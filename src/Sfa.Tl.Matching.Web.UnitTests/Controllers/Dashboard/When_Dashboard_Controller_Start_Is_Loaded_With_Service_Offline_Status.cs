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

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Dashboard
{
    public class When_Dashboard_Controller_Start_Is_Loaded_With_Service_Offline_Status
    {
        private readonly IActionResult _result;
        private readonly IEmployerService _employerService;
        private readonly IServiceStatusHistoryService _serviceStatusHistoryService;

        public When_Dashboard_Controller_Start_Is_Loaded_With_Service_Offline_Status()
        {
            _employerService = Substitute.For<IEmployerService>();
            _serviceStatusHistoryService = Substitute.For<IServiceStatusHistoryService>();
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var dashboardController = new DashboardController(_employerService, _serviceStatusHistoryService);
            var controllerWithClaims = new ClaimsBuilder<DashboardController>(dashboardController)
                .AddStandardUser()
                .AddUserName("username")
                .Build();

            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _employerService.GetInProgressEmployerOpportunityCountAsync("username").Returns(1);
            _serviceStatusHistoryService.GetLatestServiceStatusHistoryAsync()
                .Returns(new ServiceStatusHistoryViewModel
                {
                    IsOnline = false
                });

            _result = dashboardController.Start().GetAwaiter().GetResult();

        }

        [Fact]
        public void Then_GetInProgressEmployerOpportunityCount_Is_Called_Exactly_Once()
        {
            _employerService.Received(1).GetInProgressEmployerOpportunityCountAsync("username");
        }

        [Fact]
        public void Then_GetLatestServiceStatusHistory_Is_Called_Exactly_Once()
        {
            _serviceStatusHistoryService.Received(1).GetLatestServiceStatusHistoryAsync();
        }

        [Fact]
        public void Then_Controller_Returns_The_Action_Result()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void Then_ViewModel_Returns_The_Expected_Value()
        {
            var viewModel = _result.GetViewModel<DashboardViewModel>();

            viewModel.Should().NotBeNull();
            viewModel.IsServiceOnline.Should().BeFalse();
            viewModel.HasSavedOppportunities.Should().BeTrue();
            viewModel.HeaderText.Should().Be("Put service back online");
            viewModel.Description.Should().Be("Show that the service is no longer 'under maintenance'");
        }

    }
}
