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
    public class When_Employer_GetOpportunityEmployerName_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;
        private const int OpportunityId = 1;
        private const int OpportunityItemId = 12;
        private const int EmployerId = 1;

        private const string CompanyName = "CompanyName";

        public When_Employer_GetOpportunityEmployerName_Is_Loaded()
        {
            var employerService = Substitute.For<IEmployerService>();
            _opportunityService = Substitute.For<IOpportunityService>();
            
            _opportunityService.GetOpportunityEmployerAsync(OpportunityItemId)
                .Returns(new FindEmployerViewModel
                {
                    OpportunityId = OpportunityId,
                    OpportunityItemId = OpportunityItemId,
                    SelectedEmployerId = EmployerId,
                    CompanyName = CompanyName,
                });

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerDtoMapper).Assembly));
            var mapper = new Mapper(config);

            var employerController = new EmployerController(employerService, _opportunityService, mapper);

            _result = employerController.GetOpportunityEmployerName(OpportunityItemId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunityEmployer_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .GetOpportunityEmployerAsync(OpportunityItemId);
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
        public void Then_FindEmployerViewModel_Has_All_Data_Item_Set_Correctly()
        {
            var viewModel = _result.GetViewModel<FindEmployerViewModel>();
            viewModel.OpportunityId.Should().Be(OpportunityId);
            viewModel.SelectedEmployerId.Should().Be(EmployerId);
            viewModel.CompanyName.Should().Be(CompanyName);
        }
    }
}