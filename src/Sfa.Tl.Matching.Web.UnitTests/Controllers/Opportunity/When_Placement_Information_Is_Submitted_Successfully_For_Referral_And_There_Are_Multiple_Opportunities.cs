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
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_Successfully_For_Referral_And_There_Are_Multiple_Opportunities
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;

        public When_Placement_Information_Is_Submitted_Successfully_For_Referral_And_There_Are_Multiple_Opportunities()
        {
            var viewModel = new PlacementInformationSaveViewModel
            {
                OpportunityId = 1,
                OpportunityItemId = 2,
                OpportunityType = OpportunityType.Referral,
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
            _opportunityService.GetSavedOpportunityItemCountAsync(1).Returns(2);

            var checkAnswersDto = new ValidCheckAnswersDtoBuilder().Build();
            _opportunityService.GetCheckAnswers(2).Returns(checkAnswersDto);
            
            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName("username")
                .Build();

            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SavePlacementInformation(viewModel).GetAwaiter().GetResult();
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
                .Received(2)
                .GetSavedOpportunityItemCountAsync(1);
        }

        [Fact]
        public void Then_GetCheckAnswers_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetCheckAnswers(2);
        }
        
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
        public void Then_CheckAnswersViewModel_Has_All_Data_Items_Set_Correctly()
        {
            var viewModel = _result.GetViewModel<CheckAnswersViewModel>();
            viewModel.OpportunityItemId.Should().Be(2);
            viewModel.EmployerName.Should().Be("EmployerName");
            viewModel.CompanyNameAka.Should().Be("AlsoKnownAs");
            viewModel.CompanyNameWithAka.Should().Be($"EmployerName (AlsoKnownAs)");
            viewModel.JobRole.Should().Be("JobRole");
            viewModel.SearchRadius.Should().Be(3);
            viewModel.PlacementsKnown.Should().BeTrue();
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