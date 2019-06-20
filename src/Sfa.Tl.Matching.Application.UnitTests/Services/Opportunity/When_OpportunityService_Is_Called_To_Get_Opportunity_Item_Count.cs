using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Get_Opportunity_Item_Count
    {
        private readonly int _result;
        private readonly IRepository<Domain.Models.Opportunity> _opportunityRepository;

        public When_OpportunityService_Is_Called_To_Get_Opportunity_Item_Count()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);
            
            _opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            _opportunityRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>()).Returns(
                new Domain.Models.Opportunity
                {
                    Id = 1,
                    //TODO: 
                    //Count = 2
                });

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, provisionGapRepository, referralRepository);

            _result = opportunityService.GetOpportunityItemCountAsync(1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>());
        }

        [Fact]
        public void Then_Result_Count_Is_1()
        {
            _result.Should().Be(1);
        }
    }
}