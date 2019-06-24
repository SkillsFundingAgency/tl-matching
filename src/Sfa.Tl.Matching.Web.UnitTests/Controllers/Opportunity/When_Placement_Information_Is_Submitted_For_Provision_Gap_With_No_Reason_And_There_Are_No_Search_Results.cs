using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_For_Provision_Gap_With_No_Reason_And_There_Are_No_Search_Results
    {
        private readonly IActionResult _result;
        private readonly OpportunityController _opportunityController;
        private readonly IOpportunityService _opportunityService;

        public When_Placement_Information_Is_Submitted_For_Provision_Gap_With_No_Reason_And_There_Are_No_Search_Results()
        {
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.IsReferralOpportunity(1).Returns(false);
            _opportunityService.GetOpportunityItemCountAsync(1).Returns(1);

            var referralService = Substitute.For<IReferralService>();

            var viewModel = new PlacementInformationSaveViewModel
            {
                OpportunityId = 1,
                OpportunityType = OpportunityType.ProvisionGap,
                SearchResultProviderCount = 0,
                JobTitle = "Junior Tester",
                PlacementsKnown = false,
                NoSuitableStudent = false,
                HadBadExperience = false,
                ProvidersTooFarAway = false
            };

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(PlacementInformationSaveDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<PlacementInformationSaveViewModel, PlacementInformationSaveDto>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<PlacementInformationSaveViewModel, PlacementInformationSaveDto>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<PlacementInformationSaveViewModel, PlacementInformationSaveDto>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);
            
            _opportunityController = new OpportunityController(_opportunityService, referralService, mapper);

            _result = _opportunityController.PlacementInformationSave(viewModel).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_Model_State_Has_No_Errors() =>
            _opportunityController.ViewData.ModelState.Count.Should().Be(0);


        [Fact]
        public void Then_UpdateOpportunity_Is_Called_With_Expected_Field_Values()
        {
            _opportunityService
                .Received(1)
                .UpdateOpportunity(Arg.Is<PlacementInformationSaveDto>(
                    p => p.OpportunityId == 1 &&
                         p.JobTitle == "Junior Tester" &&
                         p.PlacementsKnown.HasValue &&
                         !p.PlacementsKnown.Value
            ));
        }

        [Fact]
        public void Then_IsReferralOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .IsReferralOpportunity(1);
        }

        [Fact]
        public void Then_GetOpportunityItemCountAsync_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .GetOpportunityItemCountAsync(1);
        }


        [Fact]
        public void Then_Result_Is_Redirect_To_FindEmployer()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("LoadWhoIsEmployer");
            result?.RouteValues["id"].Should().Be(1);
        }
    }
}