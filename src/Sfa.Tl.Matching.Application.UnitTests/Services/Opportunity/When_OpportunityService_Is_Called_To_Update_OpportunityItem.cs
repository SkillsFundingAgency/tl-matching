using System;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Update_Opportunity_Item
    {
        private readonly IRepository<Domain.Models.OpportunityItem> _opportunityItemRepository;

        private const int OpportunityId = 1;
        private const int OpportunityItemId = 1;

        public When_OpportunityService_Is_Called_To_Update_Opportunity_Item()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            opportunityRepository.Create(Arg.Any<Domain.Models.Opportunity>())
                .Returns(OpportunityId);

            var opportunityService = new OpportunityService(mapper, opportunityRepository, _opportunityItemRepository, provisionGapRepository, referralRepository);

            var dto = new ProviderSearchDto
            {
                OpportunityId = OpportunityId,
                OpportunityItemId = OpportunityItemId,
                RouteId = 1,
                SearchRadius = 10,
                Postcode = "OX1 1AA"
            };

            opportunityService.UpdateOpportunityItemAsync(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Update_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository.Received(1)
                .Update(Arg.Any<Domain.Models.OpportunityItem>());
        }

        [Fact]
        public void Then_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.OpportunityItem, bool>>>());
        }
    }
}