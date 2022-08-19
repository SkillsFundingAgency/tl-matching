using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_Successfully_For_Provision_Gap_And_There_Are_Multiple_Opportunities
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;

        public When_Placement_Information_Is_Submitted_Successfully_For_Provision_Gap_And_There_Are_Multiple_Opportunities()
        {
            var viewModel = new PlacementInformationSaveViewModel
            {
                OpportunityId = 1,
                OpportunityItemId = 2,
                OpportunityType = OpportunityType.ProvisionGap,
                JobRole = "Junior Tester",
                PlacementsKnown = true,
                Placements = 3,
                NoSuitableStudent = true
            };

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(PlacementInformationSaveDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                {
                    if (type.GenericTypeArguments?.Length == 2)
                    {
                        var fromTypeName = type.GenericTypeArguments[0].Name;
                        var toTypeName = type.GenericTypeArguments[1].Name;

                        if (type.Name.Contains("LoggedInUserEmailResolver"))
                        {
                            switch (fromTypeName)
                            {
                                case "PlacementInformationSaveViewModel"
                                    when toTypeName == "PlacementInformationSaveDto":
                                    return new LoggedInUserEmailResolver<PlacementInformationSaveViewModel,
                                        PlacementInformationSaveDto>(httpContextAccessor);
                                case "CheckAnswersViewModel" when toTypeName == "CheckAnswersDto":
                                    return new LoggedInUserEmailResolver<CheckAnswersViewModel, CheckAnswersDto>(
                                        httpContextAccessor);
                            }
                        }
                        else if (type.Name.Contains("LoggedInUserNameResolver"))
                        {
                            switch (fromTypeName)
                            {
                                case "PlacementInformationSaveViewModel"
                                    when toTypeName == "PlacementInformationSaveDto":
                                    return new LoggedInUserNameResolver<PlacementInformationSaveViewModel,
                                        PlacementInformationSaveDto>(httpContextAccessor);
                                case "CheckAnswersViewModel" when toTypeName == "CheckAnswersDto":
                                    return new LoggedInUserNameResolver<CheckAnswersViewModel, CheckAnswersDto>(
                                        httpContextAccessor);
                            }
                        }
                        else if (type.Name.Contains("UtcNowResolver"))
                        {
                            switch (fromTypeName)
                            {
                                case "PlacementInformationSaveViewModel"
                                    when toTypeName == "PlacementInformationSaveDto":
                                    return new UtcNowResolver<PlacementInformationSaveViewModel,
                                        PlacementInformationSaveDto>(
                                        new DateTimeProvider());
                                case "CheckAnswersViewModel" when toTypeName == "CheckAnswersDto":
                                    return new UtcNowResolver<CheckAnswersViewModel, CheckAnswersDto>(
                                        new DateTimeProvider());
                            }
                        }
                    }

                    return null;
                });
            });
            var mapper = new Mapper(config);

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetSavedOpportunityItemCountAsync(1).Returns(2);

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName("username")
                .Build();

            httpContextAccessor.HttpContext.Returns(controllerWithClaims.HttpContext);

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
        public void Then_Result_Is_Redirect_To_GetOpportunityBasket()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetOpportunityBasket");
            result?.RouteValues["opportunityId"].Should().Be(1);
            result?.RouteValues["opportunityItemId"].Should().Be(2);
        }
    }
}