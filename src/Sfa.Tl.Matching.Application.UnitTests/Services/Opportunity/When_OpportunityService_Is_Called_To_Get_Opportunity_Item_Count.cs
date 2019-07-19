using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Get_Opportunity_Item_Count
    {
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private readonly int _result;

        public When_OpportunityService_Is_Called_To_Get_Opportunity_Item_Count()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityPipelineDto>>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            _opportunityItemRepository.Count(Arg.Any<Expression<Func<OpportunityItem, bool>>>())
                .Returns(2);

            var opportunityService = new OpportunityService(mapper, opportunityRepository, _opportunityItemRepository, 
                provisionGapRepository, referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            _result = opportunityService.GetSavedOpportunityItemCountAsync(1)
                .GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_Count_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository
                .Received(1)
                .Count(Arg.Any<Expression<Func<OpportunityItem, bool>>>());
        }

        [Fact]
        public void Then_Result_Count_Is_2()
        {
            _result.Should().Be(2);
        }
    }
}