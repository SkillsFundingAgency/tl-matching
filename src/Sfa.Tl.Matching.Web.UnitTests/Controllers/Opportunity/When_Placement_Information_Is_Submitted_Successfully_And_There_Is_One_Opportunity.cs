using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_Successfully_And_There_Is_One_Opportunity
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;

        public When_Placement_Information_Is_Submitted_Successfully_And_There_Is_One_Opportunity()
        {
            var viewModel = new PlacementInformationSaveViewModel
            {
                OpportunityId = 1,
                OpportunityItemId = 2,
                JobRole = "Junior Tester",
                PlacementsKnown = true,
                Placements = 3
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
            
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetSavedOpportunityItemCountAsync(1).Returns(0);

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName("username")
                .Build();

            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SavePlacementInformationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateOpportunityItem_Is_Called_With_Expected_Field_Values()
        {
            _opportunityService
                .Received(1)
                .UpdateOpportunityItemAsync(Arg.Is<PlacementInformationSaveDto>(
                p => p.OpportunityId == 1 &&
                    p.JobRole == "Junior Tester" &&
                    p.PlacementsKnown.HasValue &&
                    p.PlacementsKnown.Value &&
                    p.Placements == 3
            ));
        }

        [Fact]
        public void Then_GetOpportunityItemCountAsync_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .GetSavedOpportunityItemCountAsync(1);
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_GetOpportunityCompanyName()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetOpportunityCompanyName");
            result?.RouteValues["opportunityId"].Should().Be(1);
            result?.RouteValues["opportunityItemId"].Should().Be(2);
        }
    }
}