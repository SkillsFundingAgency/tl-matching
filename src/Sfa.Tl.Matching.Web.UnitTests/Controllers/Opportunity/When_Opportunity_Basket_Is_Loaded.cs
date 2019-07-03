using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Opportunity_Basket_Is_Loaded
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;

        public When_Opportunity_Basket_Is_Loaded()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));

            var mapper = new Mapper(config);

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunityBasket(1).Returns(new OpportunityBasketViewModel
            {
                OpportunityId = 1,
                CompanyName = "Company Name"
            });

            var referralService = Substitute.For<IReferralService>();

            var opportunityController = new OpportunityController(_opportunityService, referralService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName("CreatedBy")
                .Build();

            _result = controllerWithClaims.OpportunityBasket(1, 1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Correct()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
            var viewModel = _result.GetViewModel<OpportunityBasketViewModel>();
            viewModel.CompanyName.Should().Be("Company Name");
        }

        [Fact]
        public void Clear_And_Load_Is_Called_Exactly_Once_In_Correct_Order()
        {
            Received.InOrder(() =>
                {
                    _opportunityService.Received(1).ClearOpportunityItemsSelectedForReferralAsync(1);
                    _opportunityService.Received(1).GetOpportunityBasket(1);

                });
        }
    }
}