using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_Start_Is_Loaded_With_No_Saved_Opportunities
    {
        private readonly IActionResult _result;

        public When_OpportunityProximity_Controller_Start_Is_Loaded_With_No_Saved_Opportunities()
        {
            var employerService = Substitute.For<IEmployerService>();
            var serviceStatusService = Substitute.For<IServiceStatusHistoryService>();

            employerService.GetInProgressEmployerOpportunityCountAsync("username").Returns(0);
            
            var dashboardController = new DashboardController(employerService, serviceStatusService);

            var controllerWithClaims = new ClaimsBuilder<DashboardController>(dashboardController)
                .Add(ClaimTypes.Role, RolesExtensions.StandardUser)
                .AddUserName("username")
                .Build();

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
            viewModel.HasSavedOppportunities.Should().BeFalse();
        }
    }
}