using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_SaveOpportunityCompanyName_Is_Submitted_Successfully : IClassFixture<EmployerControllerFixture<CompanyNameDto, FindEmployerViewModel>>
    {
        private readonly EmployerControllerFixture<CompanyNameDto, FindEmployerViewModel> _fixture;
        private readonly FindEmployerViewModel _viewModel = new FindEmployerViewModel();
        private readonly IActionResult _result;

        public When_Employer_SaveOpportunityCompanyName_Is_Submitted_Successfully(EmployerControllerFixture<CompanyNameDto, FindEmployerViewModel> fixture)
        {
            _fixture = fixture;
            _fixture = fixture;

            _viewModel.OpportunityId = _fixture.OpportunityId;
            _viewModel.OpportunityItemId = _fixture.OpportunityItemId;
            _viewModel.CompanyName = _fixture.CompanyName;
            _viewModel.SelectedEmployerCrmId = _fixture.EmployerCrmId;

            _fixture.EmployerService.ValidateCompanyNameAndCrmIdAsync(_fixture.EmployerCrmId, _fixture.CompanyName).Returns(true);
            
            _fixture.EmployerService.GetEmployerOpportunityOwnerAsync(Arg.Any<Guid>())
                .Returns((string)null);

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims(_fixture.ModifiedBy);

            _fixture.HttpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SaveOpportunityCompanyNameAsync(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetEmployer_Is_Called_Exactly_Once()
        {
            _fixture.EmployerService.Received(3).ValidateCompanyNameAndCrmIdAsync(_fixture.EmployerCrmId, _fixture.CompanyName);
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Called_Exactly_Once()
        {
            _fixture.OpportunityService.Received(1).UpdateOpportunityAsync(Arg.Any<CompanyNameDto>());
        }

        [Fact]
        public void Then_Result_Is_RedirectResult()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToRouteResult>();

            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("GetEmployerDetails");
            redirect?.RouteValues["opportunityId"].Should().Be(_fixture.OpportunityId);
            redirect?.RouteValues["opportunityItemId"].Should().Be(_fixture.OpportunityItemId);
        }
    }
}