using System;
using System.Linq.Expressions;
using AutoMapper;
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
    public class When_OpportunityService_Is_Called_To_Save_PlacementInformation_With_Unknown
    {
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private const string JobRole = "JobRole";
        private const bool PlacementsKnown = false;
        private const int Placements = 5;
        private const int OpportunityItemId = 1;
        private const string Postcode = "AA1 1AA";
        private const int Distance = 1;
        private const int RouteId = 1;

        public When_OpportunityService_Is_Called_To_Save_PlacementInformation_With_Unknown()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityPipelineDto>>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

            var opportunityItem = new OpportunityItem
            {
                Id = OpportunityItemId,
                Postcode = Postcode,
                SearchRadius = Distance,
                RouteId = RouteId
            };

            _opportunityItemRepository.GetSingleOrDefault(Arg.Any<Expression<Func<OpportunityItem, bool>>>()).Returns(opportunityItem);

            var opportunityService = new OpportunityService(mapper, opportunityRepository, _opportunityItemRepository, provisionGapRepository, 
                referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            var dto = new PlacementInformationSaveDto
            {
                OpportunityItemId = OpportunityItemId,
                JobRole = JobRole,
                PlacementsKnown = PlacementsKnown,
                Placements = Placements
            };

            opportunityService.UpdateOpportunityItemAsync(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Update_Is_Called_Exactly_Once_With_1_Placement()
        {
            _opportunityItemRepository.Received(1).Update(Arg.Is<OpportunityItem>(opportunityItem =>
                opportunityItem.Id == OpportunityItemId &&
                opportunityItem.JobRole == JobRole &&
                opportunityItem.PlacementsKnown == PlacementsKnown &&
                opportunityItem.Placements == 1));
        }

        [Fact]
        public void Then_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<OpportunityItem, bool>>>());
        }
    }
}