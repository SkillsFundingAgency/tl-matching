using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Opportunity_Controller_SaveSelectedOpportunities_Is_Called_With_ProvisionGaps_Only
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;
        private readonly OpportunityController _opportunityController;

        public When_Opportunity_Controller_SaveSelectedOpportunities_Is_Called_With_ProvisionGaps_Only()
        {
            _opportunityService = Substitute.For<IOpportunityService>();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerDtoMapper).Assembly));
            var mapper = new Mapper(config);

            _opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(_opportunityController).Build();

            _result = controllerWithClaims.SaveSelectedOpportunitiesAsync(new ContinueOpportunityViewModel
            {
                SubmitAction = "CompleteProvisionGaps",
                SelectedOpportunity = new List<SelectedOpportunityItemViewModel>
                {
                    new()
                    {
                        IsSelected = false,
                        OpportunityType = OpportunityType.ProvisionGap.ToString()
                    }
                }
            }).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_Result_Is_Redirect_To_Start()
        {
            _result.Should().NotBeNull();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().Be("Start");
        }

        [Fact]
        public void Then_OpportunityService_ContinueWithOpportunities_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).ContinueWithOpportunitiesAsync(Arg.Any<ContinueOpportunityViewModel>());
        }

        [Fact]
        public void Then_OpportunityService_ContinueWithOpportunities_Is_Not_Called()
        {
            _opportunityService.DidNotReceive().GetOpportunityBasketAsync(1);
        }

        [Fact]
        public void Then_ModelState_Is_Valid()
        {
            _opportunityController.ViewData.ModelState.IsValid.Should().BeTrue();
            _opportunityController.ViewData.ModelState.Count.Should().Be(0);
        }
    }
}