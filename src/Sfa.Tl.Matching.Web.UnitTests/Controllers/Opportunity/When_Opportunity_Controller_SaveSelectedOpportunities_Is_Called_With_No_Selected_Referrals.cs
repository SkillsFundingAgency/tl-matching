﻿using System.Collections.Generic;
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
    public class When_Opportunity_Controller_SaveSelectedOpportunities_Is_Called_With_No_Selected_Referrals
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;
        private readonly OpportunityController _opportunityController;

        public When_Opportunity_Controller_SaveSelectedOpportunities_Is_Called_With_No_Selected_Referrals()
        {
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunityBasketAsync(1)
                .Returns(new OpportunityBasketViewModel());

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerDtoMapper).Assembly));
            var mapper = new Mapper(config);

            _opportunityController = new OpportunityController(_opportunityService, mapper);

            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(_opportunityController).Build();

            _result = controllerWithClaims.SaveSelectedOpportunitiesAsync(new ContinueOpportunityViewModel
            {
                OpportunityId = 1,
                SubmitAction = "SaveSelectedOpportunities",
                SelectedOpportunity = new List<SelectedOpportunityItemViewModel>
                {
                    new()
                    {
                        IsSelected = false,
                        OpportunityType = OpportunityType.Referral.ToString()
                    }
                }
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityService_GetOpportunityBasket_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunityBasketAsync(1);
        }

        [Fact]
        public void Then_OpportunityService_ContinueWithOpportunities_Is_Not_Called()
        {
            _opportunityService.DidNotReceive().ContinueWithOpportunitiesAsync(Arg.Any<ContinueOpportunityViewModel>());
        }

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_ModelState_Is_Not_Valid()
        {
            _opportunityController.ViewData.ModelState.IsValid.Should().BeFalse();
            _opportunityController.ViewData.ModelState.Count.Should().Be(1);
            _opportunityController.ViewData.ModelState["ReferralItems[0].IsSelected"].Errors.Should().ContainSingle(error => error.ErrorMessage == "You must select an opportunity to continue");
        }
    }
}