using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_GetOpportunityCompanyName_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IEmployerService _employerService;
        private const int OpportunityId = 1;
        private const int OpportunityItemId = 12;
        private readonly Guid _employerCrmId = new Guid("11111111-1111-1111-1111-111111111111");

        private const string CompanyName = "CompanyName";

        public When_Employer_GetOpportunityCompanyName_Is_Loaded()
        {
            _employerService = Substitute.For<IEmployerService>();
            var opportunityService = Substitute.For<IOpportunityService>();
            var referralService = Substitute.For<IReferralService>();

            _employerService.GetOpportunityEmployerAsync(OpportunityId, OpportunityItemId)
                .Returns(new FindEmployerViewModel
                {
                    OpportunityId = OpportunityId,
                    OpportunityItemId = OpportunityItemId,
                    SelectedEmployerCrmId = _employerCrmId,
                    CompanyName = CompanyName
                });

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerDtoMapper).Assembly));
            var mapper = new Mapper(config);

            var employerController = new EmployerController(_employerService, opportunityService, referralService, mapper);

            _result = employerController.GetOpportunityCompanyNameAsync(OpportunityId, OpportunityItemId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunityEmployer_Is_Called_Exactly_Once()
        {
            _employerService
                .Received(1)
                .GetOpportunityEmployerAsync(OpportunityId, OpportunityItemId);
        }

        [Fact]
        public void Then_FindEmployerViewModel_Has_All_Data_Item_Set_Correctly()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();

            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<FindEmployerViewModel>();
            viewModel.OpportunityItemId.Should().Be(OpportunityItemId);
            viewModel.OpportunityId.Should().Be(OpportunityId);
            viewModel.SelectedEmployerCrmId.Should().Be(_employerCrmId);
            viewModel.CompanyName.Should().Be(CompanyName);
        }
    }
}