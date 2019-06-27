using System;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Update_ProvisionGap
    {
        private readonly IRepository<ProvisionGap> _provisionGapRepository;

        private const int OpportunityId = 1;

        public When_OpportunityService_Is_Called_To_Update_ProvisionGap()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "adminUserName")
                }))
            });

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<PlacementInformationSaveDto, ProvisionGap>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<PlacementInformationSaveDto, ProvisionGap>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<PlacementInformationSaveDto, ProvisionGap>(new DateTimeProvider()) :
                                null);
            });

            var mapper = new Mapper(config);

            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            _provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            _provisionGapRepository
                .GetSingleOrDefault(Arg.Any<Expression<Func<ProvisionGap, bool>>>())
                .Returns(new ValidProvisionGapBuilder().Build());

            opportunityRepository.Create(Arg.Any<Domain.Models.Opportunity>())
                .Returns(OpportunityId);

            var opportunityService = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository, _provisionGapRepository, referralRepository);

            var dto = new PlacementInformationSaveDto
            {
                OpportunityId = 1,
                OpportunityItemId = 1,
                OpportunityType = OpportunityType.ProvisionGap,
                SearchResultProviderCount = 0,
                JobRole = "Junior Tester",
                PlacementsKnown = false,
                NoSuitableStudent = true,
                HadBadExperience = true,
                ProvidersTooFarAway = true,
                ModifiedBy = "TestUser",
                ModifiedOn = new DateTime(2019, 1, 1)
            };

            opportunityService.UpdateProvisionGapAsync(dto).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _provisionGapRepository
                .Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<ProvisionGap, bool>>>());
        }

        [Fact]
        public void Then_Update_Is_Called_Exactly_Once()
        {
            _provisionGapRepository
                .Received(1)
                .Update(Arg.Is<ProvisionGap>(
                    p => p.OpportunityItemId ==  1 &&
                    p.NoSuitableStudent.Value &&
                    p.HadBadExperience.Value &&
                    p.ProvidersTooFarAway.Value &&
                    p.ModifiedBy == "TestUser" &&
                    p.ModifiedOn == new DateTime(2019, 1, 1)
                ));
        }
    }
}