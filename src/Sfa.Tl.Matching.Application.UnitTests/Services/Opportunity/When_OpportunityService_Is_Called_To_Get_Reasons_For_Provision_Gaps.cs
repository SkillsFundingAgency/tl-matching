using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using Castle.Core.Internal;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders;
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
        [Theory, AutoDomainData(false)]
        public async Task Then_Reason_Should_Contain_The_Values(
                                MatchingDbContext dbContext,
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
            var builder = new OpportunityItemBuilder(0, 0);
            builder.AddEmployer(0);
            builder.AddProvisionGap(0);
            var opportunityItem = builder.Build();

            opportunityItem.IsSaved = true;
            opportunityItem.IsCompleted = false;

            await dbContext.OpportunityItem.AddAsync(opportunityItem);
            await dbContext.SaveChangesAsync();

            try
            {
                var opportunityRepository = new OpportunityRepository(logger, dbContext);

                var sut = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository,
                    provisionGapRepository, referralRepository, googleMapApiClient,
                    opportunityPipelineReportWriter, dateTimeProvider);

                var result = await sut.GetOpportunityBasketAsync(opportunityItem.OpportunityId);

                result.ProvisionGapItems.Should().Contain(model => model.OpportunityType == "ProvisionGap").Which.Reason
                    .Should().ContainAny("Employer had a bad experience with them",
                        "Providers do not have students doing the right course", "Providers are too far away");
            }
            finally
            {
                ClearData(dbContext, opportunityItem.Id, opportunityItem.ProvisionGap.FirstOrDefault()?.Id);
                dbContext.Dispose();
            }
        }

        [Theory, AutoDomainData(false)]
        public async Task Then_Reasons_Should_Have_Only_Related_To_Bad_Experience(
            MatchingDbContext dbContext,
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
            var builder = new OpportunityItemBuilder(0, 0);
            builder.AddEmployer(0);
            builder.AddProvisionGap(0, false, true, false);
            var opportunityItem = builder.Build();

            opportunityItem.IsSaved = true;
            opportunityItem.IsCompleted = false;

            await dbContext.OpportunityItem.AddAsync(opportunityItem);
            await dbContext.SaveChangesAsync();

            try
            {
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
            finally
            {
                ClearData(dbContext, opportunityItem.Id, opportunityItem.ProvisionGap.FirstOrDefault()?.Id);
                dbContext.Dispose();
            }
        }

        [Theory, AutoDomainData(false)]
        public async Task Then_Reasons_Should_Have_Only_Related_To_No_Suitable_Students(
            MatchingDbContext dbContext,
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
            var builder = new OpportunityItemBuilder(0, 0);
            builder.AddEmployer(0);
            builder.AddProvisionGap(0, true, false, false);
            var opportunityItem = builder.Build();

            opportunityItem.IsSaved = true;
            opportunityItem.IsCompleted = false;

            await dbContext.OpportunityItem.AddAsync(opportunityItem);
            await dbContext.SaveChangesAsync();

            try
            {
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
            finally
            {
                ClearData(dbContext, opportunityItem.Id, opportunityItem.ProvisionGap.FirstOrDefault()?.Id);
                dbContext.Dispose();
            }
        }

        [Theory, AutoDomainData(false)]
        public async Task Then_Reasons_Should_Have_Only_Related_To_Providers_Far_Away(
            MatchingDbContext dbContext,
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
            var builder = new OpportunityItemBuilder(0, 0);
            builder.AddEmployer(0);
            builder.AddProvisionGap(0, false, false);
            var opportunityItem = builder.Build();

            opportunityItem.IsSaved = true;
            opportunityItem.IsCompleted = false;

            await dbContext.OpportunityItem.AddAsync(opportunityItem);
            await dbContext.SaveChangesAsync();

            try
            {
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
            finally
            {
                ClearData(dbContext, opportunityItem.Id, opportunityItem.ProvisionGap.FirstOrDefault()?.Id);
                dbContext.Dispose();
            }
        }

        private static void ClearData(MatchingDbContext dbContext, int opportunityItemId, int? provisionGapId)
        {
            var p = dbContext.ProvisionGap.Where(e => e.Id == provisionGapId).ToList();
            if (!p.IsNullOrEmpty()) dbContext.ProvisionGap.RemoveRange(p);

            var i = dbContext.OpportunityItem.Where(e => e.Id == opportunityItemId).ToList();
            if (!i.IsNullOrEmpty()) dbContext.OpportunityItem.RemoveRange(i);

            dbContext.SaveChanges();
        }
    }
}
