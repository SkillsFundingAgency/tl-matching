using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Loaded : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;
        
        private const bool PlacementsKnown = true;
        private const int Placements = 5;

        public When_Placement_Information_Loaded(OpportunityControllerFixture fixture)
        {
            _fixture = fixture;
            
            var dto = new PlacementInformationSaveDto
            {
                OpportunityId = _fixture.OpportunityId,
                OpportunityItemId = _fixture.OpportunityItemId,
                JobRole = _fixture.JobRole,
                OpportunityType = OpportunityType.Referral,
                CompanyName = _fixture.CompanyName,
                CompanyNameAka = _fixture.CompanyNameAka,
                PlacementsKnown = PlacementsKnown,
                Placements = Placements,
                NoSuitableStudent = true,
                HadBadExperience = true,
                ProvidersTooFarAway = true
            };

            _fixture.OpportunityService.GetPlacementInformationAsync(_fixture.OpportunityItemId).Returns(dto);

            _result = _fixture.Sut.GetPlacementInformationAsync(_fixture.OpportunityItemId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetPlacementInformation_Is_Called_Exactly_Once()
        {
            _fixture.OpportunityService.Received(2).GetPlacementInformationAsync(_fixture.OpportunityItemId);
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
            viewModel.OpportunityId.Should().Be(_fixture.OpportunityId);
            viewModel.JobRole.Should().Be(_fixture.JobRole);
            viewModel.PlacementsKnown.Should().Be(PlacementsKnown);
            viewModel.Placements.Should().Be(Placements);
            viewModel.OpportunityType.Should().Be(OpportunityType.Referral);
            viewModel.CompanyName.Should().Be(_fixture.CompanyName);
            viewModel.CompanyNameAka.Should().Be(_fixture.CompanyNameAka);
            viewModel.CompanyNameWithAka.Should().Be($"{_fixture.CompanyName} ({_fixture.CompanyNameAka})");

            viewModel.NoSuitableStudent.Should().BeTrue();
            viewModel.HadBadExperience.Should().BeTrue();
            viewModel.ProvidersTooFarAway.Should().BeTrue();
        }
    }
}