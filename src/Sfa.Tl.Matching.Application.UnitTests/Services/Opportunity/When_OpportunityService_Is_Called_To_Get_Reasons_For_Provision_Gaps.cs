using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Get_Reasons_For_Provision_Gaps
    {

        [Theory, AutoDomainData]
        public async Task Then_Reason_Should_Contain_The_Values(
                                MatchingDbContext dbContext,
                                OpportunityItem opportunityItem,
                                IMapper mapper,
                                [Frozen] ILogger<OpportunityRepository> logger,
                                [Frozen] IRepository<OpportunityItem> opportunityItemRepository,
                                [Frozen] IRepository<ProvisionGap> provisionGapRepository,
                                [Frozen] IRepository<Domain.Models.Referral> referralRepository,
                                [Frozen] IGoogleMapApiClient googleMapApiClient
        )
        {
            opportunityItem.OpportunityType = "ProvisionGap";
            opportunityItem.IsSaved = true;
            opportunityItem.IsCompleted = false;

            await dbContext.OpportunityItem.AddAsync(opportunityItem);
            await dbContext.SaveChangesAsync();

            var opportunityRepository = new OpportunityRepository(logger, dbContext);

            var sut = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository,
                provisionGapRepository, referralRepository, googleMapApiClient);

            var result = await sut.GetOpportunityBasket(opportunityItem.OpportunityId);

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().ContainAny("Employer had a bad experience with them",
                    "Providers do not have students doing the right course", "Providers were too far away");

        }

    }
}
