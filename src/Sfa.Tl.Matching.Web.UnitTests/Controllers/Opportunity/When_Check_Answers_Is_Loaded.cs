using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Check_Answers_Is_Loaded
    {
        private readonly IOpportunityService _opportunityService;
        private const string CreatedBy = "CreatedBy";
        private readonly IActionResult _result;

        private const int OpportunityId = 1;

        public When_Check_Answers_Is_Loaded()
        {
            var dto = new ValidOpportunityDtoBuilder().Build();
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunity(OpportunityId).Returns(dto);

            var opportunityController = new OpportunityController(_opportunityService);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(CreatedBy)
                .Build();

            _result = controllerWithClaims.CheckAnswers(OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunity(OpportunityId);
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
        public void Then_OpportunityId_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersViewModel>();
            viewModel.OpportunityId.Should().Be(OpportunityId);
        }

        [Fact]
        public void Then_EmployerName_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersViewModel>();
            viewModel.EmployerName.Should().Be("EmployerName");
        }

        [Fact]
        public void Then_EmployerContact_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersViewModel>();
            viewModel.Contact.Should().Be("Contact");
        }

        [Fact]
        public void Then_Distance_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersViewModel>();
            viewModel.Distance.Should().Be(3);
        }

        [Fact]
        public void Then_JobTitle_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersViewModel>();
            viewModel.JobTitle.Should().Be("JobTitle");
        }

        [Fact]
        public void Then_PlacementsKnown_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersViewModel>();
            viewModel.PlacementsKnown.Should().BeTrue();
        }

        [Fact]
        public void Then_Placements_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersViewModel>();
            viewModel.Placements.Should().Be(2);
        }

        [Fact]
        public void Then_Postcode_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersViewModel>();
            viewModel.Postcode.Should().Be("AA1 1AA");
        }

        [Fact]
        public void Then_Route_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersViewModel>();
            viewModel.Route.Should().Be("Route");
        }
    }
}