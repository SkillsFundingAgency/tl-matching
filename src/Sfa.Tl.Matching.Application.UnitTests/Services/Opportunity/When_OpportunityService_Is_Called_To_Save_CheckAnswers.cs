﻿using System;
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
    public class When_OpportunityService_Is_Called_To_Save_CheckAnswers
    {
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        private const string JobRole = "JobRole";
        private const bool PlacementsKnown = true;
        private const int Placements = 5;
        private const int OpportunityId = 10;
        private const int OpportunityItemId = 1;
        private const string Postcode = "AA1 1AA";
        private const int SearchRadius = 10;
        private const int RouteId = 1;

        public When_OpportunityService_Is_Called_To_Save_CheckAnswers()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();

            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityReportDto>>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

            var opportunityItem = new OpportunityItem
            {
                Id = OpportunityItemId,
                OpportunityId = OpportunityId,
                Postcode = Postcode,
                SearchRadius = SearchRadius,
                RouteId = RouteId,
                JobRole = JobRole,
                PlacementsKnown = PlacementsKnown,
                Placements = Placements
            };

            _opportunityItemRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<OpportunityItem, bool>>>()).Returns(opportunityItem);

            var opportunityService = new OpportunityService(mapper, opportunityRepository, _opportunityItemRepository,
                provisionGapRepository, referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            var dto = new CheckAnswersDto
            {
                OpportunityItemId = OpportunityItemId,
                OpportunityId = OpportunityId,
                IsSaved = true
            };

            opportunityService.UpdateOpportunityItemAsync(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Update_Is_Called_Exactly_Once_With_1_Placement()
        {
            _opportunityItemRepository.Received(1).UpdateAsync(Arg.Is<OpportunityItem>(opportunityItem =>
                opportunityItem.Id == OpportunityItemId &&
                opportunityItem.OpportunityId == OpportunityId &&
                opportunityItem.JobRole == JobRole &&
                opportunityItem.PlacementsKnown == PlacementsKnown &&
                opportunityItem.Placements == Placements &&
                opportunityItem.Postcode == Postcode &&
                opportunityItem.SearchRadius == SearchRadius &&
                opportunityItem.RouteId == RouteId
            ));
        }

        [Fact]
        public void Then_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository.Received(1).GetSingleOrDefaultAsync(Arg.Any<Expression<Func<OpportunityItem, bool>>>());
        }
    }
}