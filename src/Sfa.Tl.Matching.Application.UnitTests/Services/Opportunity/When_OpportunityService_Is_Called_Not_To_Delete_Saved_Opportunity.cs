using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
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

        private readonly IOpportunityRepository _opportunityRepository;
        private readonly GenericRepository<OpportunityItem> _opportunityItemRepository;
        private readonly GenericRepository<Domain.Models.Referral> _referralRepository;
        private readonly GenericRepository<ProvisionGap> _provisionGapRepository;

        public When_OpportunityService_Is_Called_Not_To_Delete_Saved_Opportunity()
        {
            var mapper = new Mapper(Substitute.For<IConfigurationProvider>());

            var opportunitylogger = Substitute.For<ILogger<GenericRepository<Domain.Models.Opportunity>>>();
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

                _opportunityItemRepository = new GenericRepository<OpportunityItem>(opportunityItemlogger, dbContext);
                _referralRepository = new GenericRepository<Domain.Models.Referral>(referralLogger, dbContext);
                _provisionGapRepository = new GenericRepository<ProvisionGap>(provisionGapLogger, dbContext);

                var service = new OpportunityService(mapper, _opportunityRepository, _opportunityItemRepository, _provisionGapRepository, _referralRepository);

                service.DeleteOpportunityItemAsync(OpportunityId, OpportunityItemId).GetAwaiter().GetResult();

            }
        }

        [Fact]
        public void Then_Do_Not_Delete_Opportunity_Item()
        {
            
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