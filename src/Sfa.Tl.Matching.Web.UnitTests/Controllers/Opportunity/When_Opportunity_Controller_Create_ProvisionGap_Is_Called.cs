using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    public class When_Opportunity_Controller_Create_ProvisionGap_Is_Called
    {
        private readonly IOpportunityService _opportunityService;
        private const string UserName = "username";
        private const string Email = "email@address.com";

        public When_Opportunity_Controller_Create_ProvisionGap_Is_Called()
        {
            const int opportunityId = 1;
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.CreateOpportunity(Arg.Any<OpportunityDto>()).Returns(opportunityId);

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            
            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(EmployerDtoMapper).Assembly);
                c.ConstructServicesUsing(type => 
                    type.Name.Contains("LoggedInUserEmailResolver") ? 
                        new LoggedInUserEmailResolver<CreateProvisionGapViewModel, OpportunityDto>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ? 
                            (object) new LoggedInUserNameResolver<CreateProvisionGapViewModel, OpportunityDto>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ? 
                                new UtcNowResolver<CreateProvisionGapViewModel, OpportunityDto>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(UserName)
                .AddEmail(Email)
                .Build();

            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);
            
            controllerWithClaims.CreateProvisionGap(new CreateProvisionGapViewModel { SearchResultProviderCount = 0, SelectedRouteId = 1, Postcode = "cv12wt", SearchRadius = 10 }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_CreateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).CreateOpportunity(Arg.Any<OpportunityDto>());
        }
    }
}