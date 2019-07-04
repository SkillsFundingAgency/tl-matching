using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Opportunity_Controller_SaveSelectedOpportunities_Is_Called_With_SubmitAction_Finish
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;

        public When_Opportunity_Controller_SaveSelectedOpportunities_Is_Called_With_SubmitAction_Finish()
        {
            _opportunityService = Substitute.For<IOpportunityService>();

            var referralService = Substitute.For<IReferralService>();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerDtoMapper).Assembly));
            var mapper = new Mapper(config);

            var opportunityController = new OpportunityController(_opportunityService, referralService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController).Build();

            _result = controllerWithClaims.SaveSelectedOpportunities(new ContinueOpportunityViewModel
            {
                SubmitAction = "Finish"
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null()
        {
            _result.Should().NotBeNull();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Start()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().Be("Start");
        }

        [Fact]
        public void Then_OpportunityService_GetOpportunityBasket_Is_Not_Called()
        {
            _opportunityService.DidNotReceive().GetOpportunityBasket(1);
        }
    }
}