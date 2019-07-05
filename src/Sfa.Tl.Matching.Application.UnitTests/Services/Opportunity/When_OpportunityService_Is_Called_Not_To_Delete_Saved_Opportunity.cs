using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_Not_To_Delete_Saved_Opportunity
    {
        private const int OpportunityId = 101;
        private const int OpportunityItemId = 1;

        private readonly int _opportunityItemCount;

        public When_OpportunityService_Is_Called_Not_To_Delete_Saved_Opportunity()
        {
            var mapper = new Mapper(Substitute.For<IConfigurationProvider>());

            var opportunitylogger = Substitute.For<ILogger<OpportunityRepository>>();
            var opportunityItemlogger = Substitute.For<ILogger<GenericRepository<OpportunityItem>>>();
            var referralLogger = Substitute.For<ILogger<GenericRepository<Domain.Models.Referral>>>();
            var provisionGapLogger = Substitute.For<ILogger<GenericRepository<ProvisionGap>>>();
            
            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(SetOpportunity());
                dbContext.AddRange(SetOpportunityItem());
                dbContext.AddRange(SetReferrals());
                dbContext.AddRange(SetProvisionGaps());
                dbContext.SaveChanges();

                var opportunityRepository = new OpportunityRepository(opportunitylogger, dbContext);
                var opportunityItemRepository = new GenericRepository<OpportunityItem>(opportunityItemlogger, dbContext);
                var referralRepository = new GenericRepository<Domain.Models.Referral>(referralLogger, dbContext);
                var provisionGapRepository = new GenericRepository<ProvisionGap>(provisionGapLogger, dbContext);

                var sut = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository, provisionGapRepository, referralRepository);

                sut.DeleteOpportunityItemAsync(OpportunityId, OpportunityItemId).GetAwaiter().GetResult();

                _opportunityItemCount = sut.GetSavedOpportunityItemCountAsync(OpportunityId).GetAwaiter().GetResult();
            }

            
        }

        [Fact]
        public void Then_Do_Not_Delete_Opportunity_Item()
        {
            _opportunityItemCount.Should().Be(2);
        }

        private static Domain.Models.Opportunity SetOpportunity()
        {
            return new Domain.Models.Opportunity
            {
                Id = OpportunityId
            };
        }

        private static IEnumerable<OpportunityItem> SetOpportunityItem()
        {
            return new List<OpportunityItem>
            {
                new OpportunityItem
                {
                    Id = 1,
                    OpportunityId = OpportunityId,
                    OpportunityType = OpportunityType.Referral.ToString(),
                    IsSelectedForReferral = false,
                    IsCompleted = false,
                    CreatedBy = "CreatedBy",
                    IsSaved = true
                },
                new OpportunityItem
                {
                    Id = 2,
                    OpportunityId = OpportunityId,
                    OpportunityType = OpportunityType.Referral.ToString(),
                    IsSelectedForReferral = false,
                    IsCompleted = false,
                    CreatedBy = "CreatedBy",
                    IsSaved = true
                }
            };
        }

        private static IEnumerable<Domain.Models.Referral> SetReferrals()
        {
            return new List<Domain.Models.Referral>
            {
                new Domain.Models.Referral
                {
                    Id = 1,
                    OpportunityItemId = OpportunityItemId,
                    OpportunityItem = SetOpportunityItem().FirstOrDefault(item => item.OpportunityId == OpportunityId)
                }
            };
        }

        private static IEnumerable<ProvisionGap> SetProvisionGaps()
        {
            return new List<ProvisionGap>
            {
                new ProvisionGap
                {
                    OpportunityItem = SetOpportunityItem().FirstOrDefault(item => item.OpportunityId == OpportunityId),
                    Id = 1,
                    OpportunityItemId = OpportunityItemId
                }
            };
        }

    }
}