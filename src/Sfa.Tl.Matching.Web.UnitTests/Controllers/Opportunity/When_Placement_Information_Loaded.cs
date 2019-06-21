using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Loaded
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;

        private readonly PlacementInformationSaveDto _dto = new PlacementInformationSaveDto();
        private const string JobTitle = "JobTitle";
        private const string CompanyName = "CompanyName";
        private const bool PlacementsKnown = true;
        private const int Placements = 5;
        private const int OpportunityId = 12;

        public When_Placement_Information_Loaded()
        {
            _dto.OpportunityId = OpportunityId;
            _dto.JobTitle = JobTitle;
            _dto.OpportunityType = OpportunityType.Referral;
            _dto.CompanyName = CompanyName;
            _dto.PlacementsKnown = PlacementsKnown;
            _dto.Placements = Placements;
            _dto.NoSuitableStudent = true;
            _dto.HadBadExperience = true;
            _dto.ProvidersTooFarAway = true;

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerStagingMapper).Assembly));
            var mapper = new Mapper(config);
            
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetPlacementInformationSave(OpportunityId).Returns(_dto);

            var referralService = Substitute.For<IReferralService>();

            var opportunityController = new OpportunityController(_opportunityService, referralService, mapper);

            _result = opportunityController.PlacementInformationSave(OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetPlacementInformationSave_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetPlacementInformationSave(OpportunityId);
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
        public void Then_ViewModel_Fields_Are_Set()
        {
            var viewModel = _result.GetViewModel<PlacementInformationSaveViewModel>();
            viewModel.OpportunityId.Should().Be(OpportunityId);
            viewModel.JobTitle.Should().Be(JobTitle);
            viewModel.PlacementsKnown.Should().Be(PlacementsKnown);
            viewModel.Placements.Should().Be(Placements);
            viewModel.OpportunityType.Should().Be(OpportunityType.Referral);
            viewModel.CompanyName.Should().Be(CompanyName);
        }

        [Fact]
        public void Then_ViewModel_Provision_Gap_Flags_Are_Set()
        {
            var viewModel = _result.GetViewModel<PlacementInformationSaveViewModel>();
            viewModel.NoSuitableStudent.Should().BeTrue();
            viewModel.HadBadExperience.Should().BeTrue();
            viewModel.ProvidersTooFarAway.Should().BeTrue();
        }
    }
}