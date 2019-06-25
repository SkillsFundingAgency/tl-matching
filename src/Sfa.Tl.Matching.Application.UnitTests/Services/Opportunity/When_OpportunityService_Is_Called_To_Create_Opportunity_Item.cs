using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Create_Opportunity_Item
    {
        private readonly int _result;
        private const int OpportunityId = 101;
        private const int OpportunityItemId = 1;

        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        public When_OpportunityService_Is_Called_To_Create_Opportunity_Item()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);
            
            var opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            _opportunityItemRepository.Create(Arg.Any<OpportunityItem>())
                .Returns(OpportunityItemId);

            var opportunityService = new OpportunityService(mapper, opportunityRepository, _opportunityItemRepository, provisionGapRepository, referralRepository);

            var dto = new OpportunityItemDto
            {
                OpportunityId = OpportunityId,
                OpportunityType = OpportunityType.Referral
            };

            _result = opportunityService.CreateOpportunityItem(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityItemRepository_Create_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository
                .Received(1)
                .Create(Arg.Is<OpportunityItem>(opportunity =>
                    opportunity.OpportunityId == OpportunityId &&
                    opportunity.OpportunityType == OpportunityType.Referral.ToString()
            ));
        }

        [Fact]
        public void Then_OpportunityItemId_Is_Created()
        {
            _result.Should().Be(OpportunityItemId);
        }
    }
}