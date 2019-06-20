﻿using System;
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
    public class When_OpportunityService_Is_Called_To_Save_PlacementInformation_With_Empty_Job_Title
    {
        private readonly IRepository<Domain.Models.Opportunity> _opportunityRepository;
        private const string JobTitle = null;
        private const bool PlacementsKnown = true;
        private const int Placements = 5;
        private const int OpportunityId = 1;
        private const string Postcode = "AA1 1AA";
        private const int Distance = 10;
        private const int RouteId = 1;

        public When_OpportunityService_Is_Called_To_Save_PlacementInformation_With_Empty_Job_Title()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);
            
            _opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            var opportunity = new Domain.Models.Opportunity { Id = OpportunityId, Postcode = Postcode, SearchRadius = Distance, RouteId = RouteId };

            _opportunityRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>()).Returns(opportunity);

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, provisionGapRepository, referralRepository);

            var dto = new PlacementInformationSaveDto
            {
                OpportunityId = OpportunityId,
                JobTitle = JobTitle,
                PlacementsKnown = PlacementsKnown,
                Placements = Placements
            };

            opportunityService.UpdateOpportunity(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Update_Is_Called_Exactly_Once_With_1_Placement()
        {
            _opportunityRepository.Received(1).Update(Arg.Is<Domain.Models.Opportunity>(opportunity => 
                opportunity.Id == OpportunityId &&
                opportunity.JobTitle == "None given" &&
                opportunity.PlacementsKnown == PlacementsKnown &&
                opportunity.Placements == Placements &&
                opportunity.Postcode == Postcode &&
                opportunity.SearchRadius == Distance &&
                opportunity.RouteId == RouteId));
        }

        [Fact]
        public void Then_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>());
        }
    }
}