using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
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
            var opportunityService = Substitute.For<IOpportunityService>();

            var viewModel = new PlacementInformationSaveViewModel
            {
                PlacementsKnown = true
            };

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);
            
            _opportunityController = new OpportunityController(opportunityService,  mapper);

            _result = _opportunityController.SavePlacementInformationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_State_Has_1_Error() =>
            _opportunityController.ViewData.ModelState.Should().ContainSingle();

        [Fact]
        public void Then_Model_State_Has_Empty_Key() =>
            _opportunityController.ViewData.ModelState.ContainsKey(nameof(PlacementInformationSaveViewModel.Placements))
                .Should().BeTrue();

        [Fact]
        public void Then_Model_State_Has_Empty_Error()
        {
            var modelStateEntry = _opportunityController.ViewData.ModelState[nameof(PlacementInformationSaveViewModel.Placements)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must estimate how many students the employer wants for this job at this location");
        }
    }
}