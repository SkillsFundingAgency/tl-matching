using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_For_Provision_Gap_With_No_Reason_And_There_Are_Search_Results
    {
        private readonly IActionResult _result;
        private readonly OpportunityController _opportunityController;

        public When_Placement_Information_Is_Submitted_For_Provision_Gap_With_No_Reason_And_There_Are_Search_Results()
        {
            var opportunityService = Substitute.For<IOpportunityService>();
            var referralService = Substitute.For<IReferralService>();

            var viewModel = new PlacementInformationSaveViewModel
            {
                OpportunityId = 1,
                OpportunityItemId = 1,
                OpportunityType = OpportunityType.ProvisionGap,
                SearchResultProviderCount = 1,
                NoSuitableStudent = false,
                HadBadExperience = false,
                ProvidersTooFarAway = false
            };

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerStagingMapper).Assembly));
            var mapper = new Mapper(config);
            
            _opportunityController = new OpportunityController(opportunityService, referralService, mapper);

            _result = _opportunityController.PlacementInformationSave(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_State_Has_1_Error() =>
            _opportunityController.ViewData.ModelState.Should().ContainSingle();

       [Fact]
        public void Then_Model_State_Has_No_Reason_Given_Key() =>
            _opportunityController.ViewData.ModelState.ContainsKey(nameof(PlacementInformationSaveViewModel.NoSuitableStudent))
                .Should().BeTrue();

        [Fact]
        public void Then_Model_State_Has_No_Reason_Given_Error()
        {
            var modelStateEntry = _opportunityController.ViewData.ModelState[nameof(PlacementInformationSaveViewModel.NoSuitableStudent)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must tell us why the employer did not choose a provider");
        }
    }
}