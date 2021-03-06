using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Loaded
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;

        private const string JobRole = "JobRole";
        private const string CompanyName = "CompanyName";
        private const string CompanyNameAka = "AlsoKnownAs";
        private const bool PlacementsKnown = true;
        private const int Placements = 5;
        private const int OpportunityId = 1;
        private const int OpportunityItemId = 12;

        public When_Placement_Information_Loaded()
        {
            var dto = new PlacementInformationSaveDto
            {
                OpportunityId = OpportunityId,
                OpportunityItemId = OpportunityItemId,
                JobRole = JobRole,
                OpportunityType = OpportunityType.Referral,
                CompanyName = CompanyName,
                CompanyNameAka = CompanyNameAka,
                PlacementsKnown = PlacementsKnown,
                Placements = Placements,
                NoSuitableStudent = true,
                HadBadExperience = true,
                ProvidersTooFarAway = true
            };

            var config = new MapperConfiguration(c => c.AddMaps(typeof(PlacementInformationSaveDtoMapper).Assembly));
            var mapper = new Mapper(config);

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetPlacementInformationAsync(OpportunityItemId).Returns(dto);

            var opportunityController = new OpportunityController(_opportunityService, mapper);

            _result = opportunityController.GetPlacementInformationAsync(OpportunityItemId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetPlacementInformation_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetPlacementInformationAsync(OpportunityItemId);
        }

        [Fact]
        public void Then_ViewModel_Fields_Are_Set()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<PlacementInformationSaveViewModel>();
            viewModel.OpportunityId.Should().Be(OpportunityId);
            viewModel.JobRole.Should().Be(JobRole);
            viewModel.PlacementsKnown.Should().Be(PlacementsKnown);
            viewModel.Placements.Should().Be(Placements);
            viewModel.OpportunityType.Should().Be(OpportunityType.Referral);
            viewModel.CompanyName.Should().Be(CompanyName);
            viewModel.CompanyNameAka.Should().Be(CompanyNameAka);
            viewModel.CompanyNameWithAka.Should().Be($"{CompanyName} ({CompanyNameAka})");

            viewModel.NoSuitableStudent.Should().BeTrue();
            viewModel.HadBadExperience.Should().BeTrue();
            viewModel.ProvidersTooFarAway.Should().BeTrue();
        }
    }
}