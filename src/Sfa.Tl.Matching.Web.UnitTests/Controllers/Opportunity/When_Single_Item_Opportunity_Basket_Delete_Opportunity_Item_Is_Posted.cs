using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Single_Item_Opportunity_Basket_Delete_Opportunity_Item_Is_Posted
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;

        public When_Single_Item_Opportunity_Basket_Delete_Opportunity_Item_Is_Posted()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));

            var mapper = new Mapper(config);

            _opportunityService = Substitute.For<IOpportunityService>();

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName("CreatedBy")
                .Build();

            _result = controllerWithClaims.DeleteOpportunityItem(new DeleteOpportunityItemViewModel { OpportunityId = 1, OpportunityItemId = 2, BasketItemCount = 1 }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Correct()
        {
            _result.Should().BeAssignableTo<RedirectToRouteResult>();
            _result.Should().NotBeNull();

            var result = _result as RedirectToRouteResult;

            result?.RouteName.Should().Be("Start");
        }

        [Fact]
        public void DeleteOpportunityItemAsync_Is_Called_Exactly_Once_In_Correct_Order()
        {
            _opportunityService.Received(1).DeleteOpportunityItemAsync(1, 2);
        }
    }
}