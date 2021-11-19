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
    public class When_Opportunity_Controller_Save_ProvisionGap_New_Opportunity
    {
        private readonly IOpportunityService _opportunityService;
        private const string UserName = "username";
        private const string Email = "email@address.com";

        private readonly IActionResult _result;

        public When_Opportunity_Controller_Save_ProvisionGap_New_Opportunity()
        {
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService
                .IsNewProvisionGapAsync(Arg.Any<int>())
                .Returns(true);

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(EmployerDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<SaveProvisionGapViewModel, OpportunityDto>(httpContextAccessor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            new LoggedInUserNameResolver<SaveProvisionGapViewModel, OpportunityDto>(httpContextAccessor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<SaveProvisionGapViewModel, OpportunityDto>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(UserName)
                .AddEmail(Email)
                .Build();

            httpContextAccessor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SaveProvisionGapAsync(new SaveProvisionGapViewModel
            {
                OpportunityId = 0,
                OpportunityItemId = 0,
                SearchResultProviderCount = 0,
                SelectedRouteId = 1,
                Postcode = "cv12wt",
                SearchRadius = 10
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Not_Called()
        {
            _opportunityService.DidNotReceive().UpdateOpportunityAsync(Arg.Any<ProviderSearchDto>());
        }

        [Fact]
        public void Then_UpdateOpportunityItem_Is_Not_Called()
        {
            _opportunityService
                .DidNotReceive()
                .UpdateOpportunityItemAsync(Arg.Any<ProviderSearchDto>());
        }

        [Fact]
        public void Then_CreateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .CreateOpportunityAsync(Arg.Any<OpportunityDto>());
        }
        
        [Fact]
        public void Then_CreateOpportunityItem_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .CreateOpportunityItemAsync(Arg.Any<OpportunityItemDto>());
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_GetPlacementInformation()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetPlacementInformation");
        }
    }
}