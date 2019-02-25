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
    public class When_Placement_Information_Loaded
    {
        private readonly IActionResult _result;
        private const int OpportunityId = 12;

        public When_Placement_Information_Loaded()
        {
            var opportunityService = Substitute.For<IOpportunityService>();

            var tempData = Substitute.For<ITempDataDictionary>();
            tempData["OpportunityId"] = OpportunityId;
            var opportunityController = new OpportunityController(opportunityService)
            {
                TempData = tempData
            };

            _result = opportunityController.Placements();
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
            var viewModel = GetViewModel();
            viewModel.OpportunityId.Should().Be(OpportunityId);
        }

        private PlacementInformationViewModel GetViewModel()
        {
            var viewResult = _result as ViewResult;
            var viewModel = viewResult?.Model as PlacementInformationViewModel;

            return viewModel;
        }
    }
}