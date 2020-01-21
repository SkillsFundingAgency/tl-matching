using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Dashboard
{
    public class When_Dashboard_Controller_Start_Is_Loaded_With_Service_Online_Status_And_No_Saved_Opportunities : IClassFixture<DashboardControllerFixture>
    {
        private readonly DashboardControllerFixture _fixture;
        private readonly IActionResult _result;
        
        public When_Dashboard_Controller_Start_Is_Loaded_With_Service_Online_Status_And_No_Saved_Opportunities(DashboardControllerFixture fixture)
        {
            _fixture = fixture;

            var controllerWithClaims = _fixture.GetControllerWithClaims;

            _fixture.HttpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _fixture.EmployerService.GetInProgressEmployerOpportunityCountAsync("username").Returns(0);
            _fixture.ServiceStatusHistoryService.GetLatestServiceStatusHistoryAsync()
                .Returns(new ServiceStatusHistoryViewModel
                {
                    IsOnline = true
                });

            _result = _fixture.SubjectUnderTest.Start().GetAwaiter().GetResult();

        }

        [Fact]
        public void Then_GetInProgressEmployerOpportunityCount_Is_Called_Exactly_Once()
        {
            _fixture.EmployerService.Received(2).GetInProgressEmployerOpportunityCountAsync("username");
        }

        [Fact]
        public void Then_GetLatestServiceStatusHistory_Is_Called_Exactly_Once()
        {
            _fixture.ServiceStatusHistoryService.Received(4).GetLatestServiceStatusHistoryAsync();
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
            viewModel.IsServiceOnline.Should().BeTrue();
            viewModel.HasSavedOppportunities.Should().BeFalse();
            viewModel.HeaderText.Should().Be("Take service offline");
            viewModel.Description.Should().Be("Show that the service is 'under maintenance'.");
        }

    }
}
