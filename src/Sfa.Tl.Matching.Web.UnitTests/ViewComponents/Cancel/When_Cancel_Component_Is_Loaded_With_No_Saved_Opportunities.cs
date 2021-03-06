﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.ViewComponents;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.ViewComponents.Cancel
{
    public class When_Cancel_Component_Is_Loaded_With_No_Saved_Opportunities
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IViewComponentResult _result;

        public When_Cancel_Component_Is_Loaded_With_No_Saved_Opportunities()
        {
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetSavedOpportunityItemCountAsync(1).Returns(0);

            var viewComponent = new CancelViewComponent(_opportunityService);

            _result = viewComponent.InvokeAsync(1, 2).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityService_GetSavedOpportunityItemCountAsync_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .GetSavedOpportunityItemCountAsync(1);
        }

        [Fact]
        public void Then_ViewComponentResult_Has_Expected_Values()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewViewComponentResult>();

            var viewViewComponentResult = _result as ViewViewComponentResult;
            viewViewComponentResult.Should().NotBeNull();

            viewViewComponentResult?.ViewName.Should().Be("Cancel");
            viewViewComponentResult?.ViewData.Model.Should().NotBeNull();
            var viewModel = viewViewComponentResult?.ViewData.Model.As<CancelViewModel>();
            viewModel.Should().NotBeNull(); 
            viewModel?.CancelText.Should().Be("Cancel opportunity and start again");
        }
    }
}