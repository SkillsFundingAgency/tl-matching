using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
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
                .GetManyAsync(Arg.Any<Expression<Func<OpportunityItem, bool>>>())
                .Returns(SetOpportunityItem().AsQueryable());

            _referralRepository
                .GetManyAsync(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>())
                .Returns(SetReferrals().AsQueryable());

            _provisionGapRepository
                .GetManyAsync(Arg.Any<Expression<Func<ProvisionGap, bool>>>())
                .Returns(SetProvisionGaps().AsQueryable());

            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityReportDto>>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, _opportunityItemRepository, 
                _provisionGapRepository, _referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            opportunityService.DeleteOpportunityItemAsync(OpportunityId, OpportunityItemId).GetAwaiter().GetResult();

        }

        [Fact]
        public void Then_Delete_Opportunity_Is_Called_Exactly_Once()
        {
            _opportunityRepository.Received(1).DeleteAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_Delete_Opportunity_Item_Is_Called_Twice()
        {
            _opportunityItemRepository.Received(2).DeleteAsync(Arg.Any<OpportunityItem>());
        }

        [Fact]
        public void Then_Delete_Referral_Is_Called_Thrice()
        {
            _referralRepository.Received(3).DeleteManyAsync(Arg.Any<List<Domain.Models.Referral>>());
        }

        [Fact]
        public void Then_Delete_Provision_Gap_Is_Called_Thrice()
        {
            _provisionGapRepository.Received(3).DeleteManyAsync(Arg.Any<List<ProvisionGap>>());
        }

        private static IEnumerable<OpportunityItem> SetOpportunityItem()
        {
            return new List<OpportunityItem>
            {
                new()
                {
                    Id = 1,
                    OpportunityId = OpportunityId,
                    OpportunityType = OpportunityType.Referral.ToString(),
                    IsSelectedForReferral = false,
                    IsCompleted = false,
                    CreatedBy = "CreatedBy",
                    IsSaved = false
                },
                new()
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
                new()
                {
                    Id = 1,
                    OpportunityItemId = OpportunityItemId
                }
            };
        }

        private static IEnumerable<ProvisionGap> SetProvisionGaps()
        {
            return new List<ProvisionGap>
            {
                new()
                {
                    Id = 1,
                    OpportunityItemId = OpportunityItemId
                }
            };
        }

    }
}