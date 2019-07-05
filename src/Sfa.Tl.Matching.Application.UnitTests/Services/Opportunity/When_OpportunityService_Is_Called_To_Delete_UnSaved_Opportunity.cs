using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Delete_UnSaved_Opportunity
    {
        private const int OpportunityId = 101;
        private const int OpportunityItemId = 1;

        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private readonly IRepository<Domain.Models.Referral> _referralRepository;
        private readonly IRepository<ProvisionGap> _provisionGapRepository;

        public When_OpportunityService_Is_Called_To_Delete_UnSaved_Opportunity()
        {
            var mapper = new Mapper(Substitute.For<IConfigurationProvider>());

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            _provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            _referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            _opportunityItemRepository
                .GetMany(Arg.Any<Expression<Func<OpportunityItem, bool>>>())
                .Returns(SetOpportunityItem().AsQueryable());

            _referralRepository
                .GetMany(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>())
                .Returns(SetReferrals().AsQueryable());

            _provisionGapRepository
                .GetMany(Arg.Any<Expression<Func<ProvisionGap, bool>>>())
                .Returns(SetProvisionGaps().AsQueryable());

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, _opportunityItemRepository, _provisionGapRepository, _referralRepository);

            opportunityService.DeleteOpportunityItemAsync(OpportunityId, OpportunityItemId).GetAwaiter().GetResult();

        }

        [Fact]
        public void Then_Delete_Opportunity_Is_Called_Exactly_Once()
        {
            _opportunityRepository.Received(1).Delete(Arg.Any<int>());
        }

        [Fact]
        public void Then_Delete_Opportunity_Item_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository.Received(1).DeleteMany(Arg.Any<List<OpportunityItem>>());
        }

        [Fact]
        public void Then_Delete_Referral_Is_Called_Exactly_Once()
        {
            _referralRepository.Received(1).DeleteMany(Arg.Any<List<Domain.Models.Referral>>());
        }

        [Fact]
        public void Then_Delete_Provision_Gap_Is_Called_Exactly_Once()
        {
            _provisionGapRepository.Received(1).DeleteMany(Arg.Any<List<ProvisionGap>>());
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
                    IsSaved = false
                },
                new OpportunityItem
                {
                    Id = 2,
                    OpportunityId = OpportunityId,
                    OpportunityType = OpportunityType.Referral.ToString(),
                    IsSelectedForReferral = false,
                    IsCompleted = false,
                    CreatedBy = "CreatedBy",
                    IsSaved = false
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