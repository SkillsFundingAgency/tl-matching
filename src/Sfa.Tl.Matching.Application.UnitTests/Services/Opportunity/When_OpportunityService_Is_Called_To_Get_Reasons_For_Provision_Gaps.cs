using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
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
                                [Frozen] IGoogleMapApiClient googleMapApiClient,
                                [Frozen] IFileWriter<OpportunityPipelineDto> opportunityPipelineReportWriter
        )
        {
            opportunityItem.OpportunityType = "ProvisionGap";
            opportunityItem.IsSaved = true;
            opportunityItem.IsCompleted = false;

            await dbContext.OpportunityItem.AddAsync(opportunityItem);
            await dbContext.SaveChangesAsync();

            await SetProvisionGapData(dbContext, true, false, true);

            var opportunityRepository = new OpportunityRepository(logger, dbContext);

            var sut = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository,
                provisionGapRepository, referralRepository, googleMapApiClient, opportunityPipelineReportWriter);

            var result = await sut.GetOpportunityBasket(opportunityItem.OpportunityId);

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().ContainAny("Employer had a bad experience with them",
                    "Providers do not have students doing the right course", "Providers were too far away");

        }

        [Theory, AutoDomainData]
        public async Task Then_Reasons_Should_Have_Only_Related_To_Bad_Experience(
            MatchingDbContext dbContext,
            OpportunityItem opportunityItem,
            IMapper mapper,
            [Frozen] ILogger<OpportunityRepository> logger,
            [Frozen] IRepository<OpportunityItem> opportunityItemRepository,
            [Frozen] IRepository<ProvisionGap> provisionGapRepository,
            [Frozen] IRepository<Domain.Models.Referral> referralRepository,
            [Frozen] IGoogleMapApiClient googleMapApiClient,
            [Frozen] IFileWriter<OpportunityPipelineDto> opportunityPipelineReportWriter
        )
        {
            opportunityItem.OpportunityType = "ProvisionGap";
            opportunityItem.IsSaved = true;
            opportunityItem.IsCompleted = false;

            await dbContext.OpportunityItem.AddAsync(opportunityItem);
            await dbContext.SaveChangesAsync();

            await SetProvisionGapData(dbContext, true, false, false);
            
            var opportunityRepository = new OpportunityRepository(logger, dbContext);

            var sut = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository,
                provisionGapRepository, referralRepository, googleMapApiClient, opportunityPipelineReportWriter);

            var result = await sut.GetOpportunityBasket(opportunityItem.OpportunityId);

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().Contain("Employer had a bad experience with them");

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().NotContain("Providers were too far away");

        }

        [Theory, AutoDomainData]
        public async Task Then_Reasons_Should_Have_Only_Related_To_No_Suitable_Students(
            MatchingDbContext dbContext,
            OpportunityItem opportunityItem,
            IMapper mapper,
            [Frozen] ILogger<OpportunityRepository> logger,
            [Frozen] IRepository<OpportunityItem> opportunityItemRepository,
            [Frozen] IRepository<ProvisionGap> provisionGapRepository,
            [Frozen] IRepository<Domain.Models.Referral> referralRepository,
            [Frozen] IGoogleMapApiClient googleMapApiClient,
            [Frozen] IFileWriter<OpportunityPipelineDto> opportunityPipelineReportWriter
        )
        {
            opportunityItem.OpportunityType = "ProvisionGap";
            opportunityItem.IsSaved = true;
            opportunityItem.IsCompleted = false;

            await dbContext.OpportunityItem.AddAsync(opportunityItem);
            await dbContext.SaveChangesAsync();

            await SetProvisionGapData(dbContext, false, true, false);

            var opportunityRepository = new OpportunityRepository(logger, dbContext);

            var sut = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository,
                provisionGapRepository, referralRepository, googleMapApiClient, opportunityPipelineReportWriter);

            var result = await sut.GetOpportunityBasket(opportunityItem.OpportunityId);

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().Contain("Providers do not have students doing the right course");

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().NotContain("Employer had a bad experience with them");

        }

        [Theory, AutoDomainData]
        public async Task Then_Reasons_Should_Have_Only_Related_To_Providers_Far_Away(
            MatchingDbContext dbContext,
            OpportunityItem opportunityItem,
            IMapper mapper,
            [Frozen] ILogger<OpportunityRepository> logger,
            [Frozen] IRepository<OpportunityItem> opportunityItemRepository,
            [Frozen] IRepository<ProvisionGap> provisionGapRepository,
            [Frozen] IRepository<Domain.Models.Referral> referralRepository,
            [Frozen] IGoogleMapApiClient googleMapApiClient,
            [Frozen] IFileWriter<OpportunityPipelineDto> opportunityPipelineReportWriter
        )
        {
            opportunityItem.OpportunityType = "ProvisionGap";
            opportunityItem.IsSaved = true;
            opportunityItem.IsCompleted = false;

            await dbContext.OpportunityItem.AddAsync(opportunityItem);
            await dbContext.SaveChangesAsync();

            await SetProvisionGapData(dbContext, false, false, true);

            var opportunityRepository = new OpportunityRepository(logger, dbContext);

            var sut = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository,
                provisionGapRepository, referralRepository, googleMapApiClient, opportunityPipelineReportWriter);

            var result = await sut.GetOpportunityBasket(opportunityItem.OpportunityId);

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().Contain("Providers were too far away");

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().NotContain("Providers do not have students doing the right course");

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().NotContain("Employer had a bad experience with them");

        }

        private static async Task SetProvisionGapData(
                            MatchingDbContext dbContext, 
                            bool hasBadExperience, 
                            bool hasNosuitableStudent, 
                            bool areProvidersTooFarAway)
        {
            var provisionGap = await dbContext.ProvisionGap.FirstOrDefaultAsync();

            provisionGap.HadBadExperience = hasBadExperience;
            provisionGap.NoSuitableStudent = hasNosuitableStudent;
            provisionGap.ProvidersTooFarAway = areProvidersTooFarAway;

            dbContext.Entry(provisionGap).Property("HadBadExperience").IsModified = true;
            dbContext.Entry(provisionGap).Property("NoSuitableStudent").IsModified = true;
            dbContext.Entry(provisionGap).Property("ProvidersTooFarAway").IsModified = true;

            await dbContext.SaveChangesAsync();
        }

    }
}
