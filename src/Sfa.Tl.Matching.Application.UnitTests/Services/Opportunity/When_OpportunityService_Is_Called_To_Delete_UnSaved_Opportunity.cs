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

        public When_OpportunityService_Is_Called_To_Delete_UnSaved_Opportunity()
        {
            var mapper = new Mapper(Substitute.For<IConfigurationProvider>());

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

           _opportunityItemRepository
                .GetMany(Arg.Any<Expression<Func<OpportunityItem, bool>>>())
                .Returns(new List<OpportunityItem>
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
                    }
                }.AsQueryable());

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, _opportunityItemRepository, provisionGapRepository, referralRepository);

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
    }
}