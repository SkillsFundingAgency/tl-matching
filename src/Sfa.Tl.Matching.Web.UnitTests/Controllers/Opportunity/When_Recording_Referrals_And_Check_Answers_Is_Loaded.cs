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
    public class When_Recording_Referrals_And_Check_Answers_Is_Loaded
    {
        private readonly IOpportunityService _opportunityService;
        private const string CreatedBy = "CreatedBy";
        private readonly IActionResult _result;

        private const int OpportunityId = 1;

        public When_Recording_Referrals_And_Check_Answers_Is_Loaded()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);
            
            var dto = new ValidOpportunityDtoBuilder().Build();
            var providers = new List<ReferralsViewModel>
            {
                new ReferralsViewModel { Name = "Provider1", DistanceFromEmployer = 1.3m, Postcode = "AA1 1AA" },
                new ReferralsViewModel { Name = "Provider2", DistanceFromEmployer = 31.6m, Postcode = "BB1 1BB" }
            };

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunityWithRoute(OpportunityId).Returns(dto);

            _opportunityService.GetReferrals(OpportunityId).Returns(providers);

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(CreatedBy)
                .Build();

            _result = controllerWithClaims.CheckAnswersReferrals(OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunityWithRoute_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunityWithRoute(OpportunityId);
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
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.OpportunityId.Should().Be(OpportunityId);
        }

        [Fact]
        public void Then_EmployerName_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.PlacementInformation.EmployerName.Should().Be("CompanyName");
        }

        [Fact]
        public void Then_EmployerContact_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.PlacementInformation.Contact.Should().Be("Contact");
        }

        [Fact]
        public void Then_Distance_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.PlacementInformation.Distance.Should().Be(3);
        }

        [Fact]
        public void Then_JobTitle_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.PlacementInformation.JobTitle.Should().Be("JobTitle");
        }

        [Fact]
        public void Then_PlacementsKnown_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.PlacementInformation.PlacementsKnown.Should().BeTrue();
        }

        [Fact]
        public void Then_Placements_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.PlacementInformation.Placements.Should().Be(2);
        }

        [Fact]
        public void Then_Postcode_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.PlacementInformation.Postcode.Should().Be("AA1 1AA");
        }

        [Fact]
        public void Then_Route_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.PlacementInformation.RouteName.Should().Be("RouteName");
        }

        [Fact]
        public void Then_Providers_Count_Is_2()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.Providers.Count.Should().Be(2);
        }

        [Fact]
        public void Then_First_Provider_Name_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.Providers[0].Name.Should().Be("Provider1");
        }

        [Fact]
        public void Then_First_Provider_Distance_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.Providers[0].DistanceFromEmployer.Should().Be(1.3m);
        }

        [Fact]
        public void Then_First_Provider_Postcode_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.Providers[0].Postcode.Should().Be("AA1 1AA");
        }

        [Fact]
        public void Then_Second_Provider_Name_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.Providers[1].Name.Should().Be("Provider2");
        }

        [Fact]
        public void Then_Second_Provider_Distance_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.Providers[1].DistanceFromEmployer.Should().Be(31.6m);
        }

        [Fact]
        public void Then_Second_Provider_Postcode_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersReferralViewModel>();
            viewModel.Providers[1].Postcode.Should().Be("BB1 1BB");
        }
    }
}