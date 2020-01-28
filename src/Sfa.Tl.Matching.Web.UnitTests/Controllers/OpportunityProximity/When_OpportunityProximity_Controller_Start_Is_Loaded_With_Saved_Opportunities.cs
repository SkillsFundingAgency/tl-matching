using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_Start_Is_Loaded_With_Saved_Opportunities : IClassFixture<DashboardControllerFixture>
    {
        private readonly IActionResult _result;

        public When_OpportunityProximity_Controller_Start_Is_Loaded_With_Saved_Opportunities()
        {
            var fixture = new DashboardControllerFixture();

            fixture.EmployerService.GetInProgressEmployerOpportunityCountAsync("username").Returns(1);
            fixture.ServiceStatusHistoryService.GetLatestServiceStatusHistoryAsync().Returns(new ServiceStatusHistoryViewModel
            {
                IsOnline = false
            });

            var controllerWithClaims = fixture.SubjectUnderTest.ControllerWithClaims("username");

            _result = controllerWithClaims.Start().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Correct()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();

            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<DashboardViewModel>();
            viewModel.HasSavedOppportunities.Should().BeTrue();
        }
    }
}