﻿using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Get_Placement_Information_With_Provision_Gap
    {
        private readonly PlacementInformationSaveDto _result;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        public When_OpportunityService_Is_Called_To_Get_Placement_Information_With_Provision_Gap()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            var dto = new OpportunityItemBuilder()
                .AddProvisionGap()
                .AddEmployer()
                .Build();

            _opportunityItemRepository.GetSingleOrDefault(Arg.Any<Expression<Func<OpportunityItem, bool>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>())
                .Returns(dto);
            
            var opportunityService = new OpportunityService(mapper, opportunityRepository, _opportunityItemRepository,
                provisionGapRepository, referralRepository);

            _result = opportunityService.GetPlacementInformationSaveAsync(1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository
                .Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<OpportunityItem, bool>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>());
        }

        [Fact]
        public void Then_Result_Fields_Are_Correct()
        {
            _result.CompanyName.Should().Be("CompanyName");
            _result.OpportunityType.Should().Be(OpportunityType.ProvisionGap);
            _result.NoSuitableStudent.Should().BeTrue();
            _result.HadBadExperience.Should().BeTrue();
            _result.ProvidersTooFarAway.Should().BeTrue();
        }
    }
}