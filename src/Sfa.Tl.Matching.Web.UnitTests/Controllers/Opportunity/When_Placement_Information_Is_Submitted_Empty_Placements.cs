using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_Empty_Placements
    {
        private readonly IActionResult _result;
        private readonly OpportunityController _opportunityController;

        public When_Placement_Information_Is_Submitted_Empty_Placements()
        {
            var mapper = Substitute.For<IMapper>();
            var opportunityService = Substitute.For<IOpportunityService>();
            
            var viewModel = new PlacementInformationViewModel
            {
                PlacementsKnown = true
            };

            var tempData = Substitute.For<ITempDataDictionary>();
            _opportunityController = new OpportunityController(mapper, opportunityService)
            {
                TempData = tempData
            };

            _result = _opportunityController.Placements(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_State_Has_1_Error() =>
            _opportunityController.ViewData.ModelState.Should().ContainSingle();

        [Fact]
        public void Then_Model_State_Has_Empty_Key() =>
            _opportunityController.ViewData.ModelState.ContainsKey(nameof(PlacementInformationViewModel.Placements))
                .Should().BeTrue();

        [Fact]
        public void Then_Model_State_Has_Empty_Error()
        {
            var modelStateEntry = _opportunityController.ViewData.ModelState[nameof(PlacementInformationViewModel.Placements)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must estimate how many placements the employer wants at this location");
        }
    }
}