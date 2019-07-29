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
    public class When_MultiItem_Opportunity_Basket_Confirm_Delete_Opportunity_Item_Is_Loaded
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;

        public When_MultiItem_Opportunity_Basket_Confirm_Delete_Opportunity_Item_Is_Loaded()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));

            var mapper = new Mapper(config);

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetConfirmDeleteOpportunityItemAsync(1).Returns(new ConfirmDeleteOpportunityItemViewModel
            {
                OpportunityItemId = 1,
                OpportunityId = 2,
                CompanyName = "Company Name1",
                CompanyNameAka = "Also Known As 1",
                Postcode = "PostCode1",
                JobRole = "JobRole1",
                BasketItemCount = 2,
            });

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName("CreatedBy")
                .Build();

            _result = controllerWithClaims.ConfirmDeleteOpportunityItem(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Correct()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
            var viewModel = _result.GetViewModel<ConfirmDeleteOpportunityItemViewModel>();
            viewModel.OpportunityItemId.Should().Be(1);
            viewModel.OpportunityId.Should().Be(2);
            viewModel.CompanyName.Should().Be("Company Name1");
            viewModel.CompanyNameAka.Should().Be("Also Known As 1");
            viewModel.CompanyNameWithAka.Should().Be("Company Name1 (Also Known As 1)");
            viewModel.Postcode.Should().Be("PostCode1");
            viewModel.JobRole.Should().Be("JobRole1");
            viewModel.PlacementsDetail.Should().Be("at least 1");
            viewModel.DeleteWarningText.Should().Be("This cannot be undone.");
            viewModel.SubmitActionText.Should().Be("Confirm and continue");
        }

        [Fact]
        public void GetConfirmDeleteOpportunityItemAsync_Is_Called_Exactly_Once_In_Correct_Order()
        {
            _opportunityService.Received(1).GetConfirmDeleteOpportunityItemAsync(1);
        }
    }
}