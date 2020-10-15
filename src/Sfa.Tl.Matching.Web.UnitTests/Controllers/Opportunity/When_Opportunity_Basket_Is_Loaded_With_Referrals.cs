using System.Collections.Generic;
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
    public class When_Opportunity_Basket_Is_Loaded_With_Referrals
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;

        public When_Opportunity_Basket_Is_Loaded_With_Referrals()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));

            var mapper = new Mapper(config);

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunityBasketAsync(1).Returns(new OpportunityBasketViewModel
            {
                OpportunityId = 1,
                CompanyName = "Company Name",
                CompanyNameAka = "Also Known As",
                ProvisionGapItems = null,
                ReferralItems = new List<BasketReferralItemViewModel>
                {
                    new BasketReferralItemViewModel
                    {
                        OpportunityType = "Referral",
                        OpportunityItemId = 5
                    }
                }
            });

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName("CreatedBy")
                .Build();

            _result = controllerWithClaims.GetOpportunityBasketAsync(1, 0).GetAwaiter().GetResult();
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
            viewModel.CompanyNameAka.Should().Be("Also Known As");
            viewModel.CompanyNameWithAka.Should().Be("Company Name (Also Known As)");
            viewModel.OpportunityId.Should().Be(1);
            viewModel.OpportunityItemId.Should().Be(5);
        }

        [Fact]
        public void Clear_And_Load_Is_Called_Exactly_Once_In_Correct_Order()
        {
            Received.InOrder(() =>
                {
                    _opportunityService.Received(1).ClearOpportunityItemsSelectedForReferralAsync(1);
                    _opportunityService.Received(1).GetOpportunityBasketAsync(1);
                });
        }
    }
}