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
                                [Frozen] IFileWriter<OpportunityReportDto> opportunityPipelineReportWriter,
                                [Frozen] IDateTimeProvider dateTimeProvider
        )
        {
            await SetProvisionGapData(dbContext, opportunityItem, true, false, true);

            var opportunityRepository = new OpportunityRepository(logger, dbContext);

            var sut = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository,
                provisionGapRepository, referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            var result = await sut.GetOpportunityBasketAsync(opportunityItem.OpportunityId);

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().ContainAny("Employer had a bad experience with them",
                    "Providers do not have students doing the right course", "Providers are too far away");
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
            [Frozen] IFileWriter<OpportunityReportDto> opportunityPipelineReportWriter,
            [Frozen] IDateTimeProvider dateTimeProvider
        )
        {
            await SetProvisionGapData(dbContext, opportunityItem, true, false, false);

            var opportunityRepository = new OpportunityRepository(logger, dbContext);

            var sut = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository,
                provisionGapRepository, referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            var result = await sut.GetOpportunityBasketAsync(opportunityItem.OpportunityId);

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().Contain("Employer had a bad experience with them");

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().NotContain("Providers are too far away");
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
            [Frozen] IFileWriter<OpportunityReportDto> opportunityPipelineReportWriter,
            [Frozen] IDateTimeProvider dateTimeProvider
        )
        {
            await SetProvisionGapData(dbContext, opportunityItem, false, true, false);

            var opportunityRepository = new OpportunityRepository(logger, dbContext);

            var sut = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository,
                provisionGapRepository, referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            var result = await sut.GetOpportunityBasketAsync(opportunityItem.OpportunityId);

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
            [Frozen] IFileWriter<OpportunityReportDto> opportunityPipelineReportWriter,
            [Frozen] IDateTimeProvider dateTimeProvider
        )
        {
            await SetProvisionGapData(dbContext, opportunityItem, false, false, true);

            var opportunityRepository = new OpportunityRepository(logger, dbContext);

            var sut = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository,
                provisionGapRepository, referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            var result = await sut.GetOpportunityBasketAsync(opportunityItem.OpportunityId);

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().Contain("Providers are too far away");

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().NotContain("Providers do not have students doing the right course");

            result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                .Should().NotContain("Employer had a bad experience with them");
        }

        private static async Task SetProvisionGapData(
                            MatchingDbContext dbContext,
                            OpportunityItem opportunityItem,
                            bool hasBadExperience,
                            bool hasNosuitableStudent,
                            bool areProvidersTooFarAway)
        {
            opportunityItem.OpportunityType = "ProvisionGap";
            opportunityItem.IsSaved = true;
            opportunityItem.IsCompleted = false;

            //Need to remove extra referral/provision gap rows created as AutoDomainData
            foreach (var x in opportunityItem.ProvisionGap.Where(pg => pg.Id != opportunityItem.ProvisionGap.First().Id).ToList())
            {
                opportunityItem.ProvisionGap.Remove(x);
            }
            opportunityItem.Referral.Clear();

            await dbContext.OpportunityItem.AddAsync(opportunityItem);
            await dbContext.SaveChangesAsync();

            //Set up the provision gap record
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
