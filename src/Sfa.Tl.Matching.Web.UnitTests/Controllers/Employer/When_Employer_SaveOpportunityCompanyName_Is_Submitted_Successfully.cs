using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_SaveOpportunityCompanyName_Is_Submitted_Successfully
    {
        private readonly IEmployerService _employerService;
        private readonly IOpportunityService _opportunityService;
        private const string CompanyName = "CompanyName";
        private const string ModifiedBy = "ModifiedBy";
        private readonly FindEmployerViewModel _viewModel = new FindEmployerViewModel();
        private readonly IActionResult _result;

        private const int OpportunityId = 1;
        private const int OpportunityItemId = 2;
        private readonly Guid _employerCrmId = new Guid("33333333-3333-3333-3333-333333333333");

        public When_Employer_SaveOpportunityCompanyName_Is_Submitted_Successfully()
        {
            _viewModel.OpportunityId = OpportunityId;
            _viewModel.OpportunityItemId = OpportunityItemId;
            _viewModel.CompanyName = CompanyName;
            _viewModel.SelectedEmployerCrmId = new Guid("33333333-3333-3333-3333-333333333333");

            _employerService = Substitute.For<IEmployerService>();
            _employerService.ValidateCompanyNameAndCrmIdAsync(_employerCrmId, CompanyName).Returns(true);
            _employerService.GetEmployerOpportunityOwnerAsync(Arg.Any<Guid>())
                .Returns((string)null);

            _opportunityService = Substitute.For<IOpportunityService>();
            var referralService = Substitute.For<IReferralService>();

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(EmployerDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<FindEmployerViewModel, CompanyNameDto>(httpcontextAccesor) :
                            type.Name.Contains("LoggedInUserNameResolver") ?
                                (object)new LoggedInUserNameResolver<FindEmployerViewModel, CompanyNameDto>(httpcontextAccesor) :
                                    type.Name.Contains("UtcNowResolver") ?
                                        new UtcNowResolver<FindEmployerViewModel, CompanyNameDto>(new DateTimeProvider()) :
                                            null);
            });

            var mapper = new Mapper(config);

            var employerController = new EmployerController(_employerService, _opportunityService, referralService, mapper);
            var controllerWithClaims = new ClaimsBuilder<EmployerController>(employerController)
                .AddUserName(ModifiedBy)
                .Build();

            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SaveOpportunityCompanyNameAsync(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetEmployer_Is_Called_Exactly_Once()
        {
            _employerService.Received(1).ValidateCompanyNameAndCrmIdAsync(_employerCrmId, CompanyName);
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).UpdateOpportunityAsync(Arg.Any<CompanyNameDto>());
        }

        [Fact]
        public void Then_Result_Is_RedirectResult() =>
            _result.Should().BeOfType<RedirectToRouteResult>();

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo("GetEmployerDetails");
            redirect?.RouteValues["opportunityId"].Should().Be(1);
            redirect?.RouteValues["opportunityItemId"].Should().Be(2);
        }
    }
}