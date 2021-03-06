﻿using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
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

        private const int OpportunityItemId = 2;

        public When_Recording_Referrals_And_Check_Answers_Is_Loaded()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(CheckAnswersDtoMapper).Assembly));
            var mapper = new Mapper(config);

            var dto = new ValidCheckAnswersDtoBuilder().Build();

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetCheckAnswersAsync(OpportunityItemId).Returns(dto);

            var opportunityController = new OpportunityController(_opportunityService,  mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(CreatedBy)
                .Build();

            _result = controllerWithClaims.GetCheckAnswersAsync(OpportunityItemId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetCheckAnswers_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetCheckAnswersAsync(OpportunityItemId);
        }
        
        [Fact]
        public void Then_CheckAnswersViewModel_Has_All_Data_Items_Set_Correctly()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<CheckAnswersViewModel>();
            viewModel.OpportunityItemId.Should().Be(OpportunityItemId);
            viewModel.CompanyName.Should().Be("CompanyName");
            viewModel.CompanyNameAka.Should().Be("AlsoKnownAs");
            viewModel.CompanyNameWithAka.Should().Be("CompanyName (AlsoKnownAs)");
            viewModel.JobRole.Should().Be("JobRole");
            viewModel.SearchRadius.Should().Be(3);
            viewModel.Placements.Should().Be(2);
            viewModel.Postcode.Should().Be("AA1 1AA");
            viewModel.RouteName.Should().Be("RouteName");
            
            viewModel.Providers.Count.Should().Be(2);

            viewModel.Providers[0].Name.Should().Be("Provider1");
            viewModel.Providers[0].DistanceFromEmployer.Should().Be(1.3m);
            viewModel.Providers[0].Postcode.Should().Be("AA1 1AA");
            viewModel.Providers[1].Name.Should().Be("Provider2");
            viewModel.Providers[1].DistanceFromEmployer.Should().Be(31.6m);
            viewModel.Providers[1].Postcode.Should().Be("BB1 1BB");
        }
    }
}