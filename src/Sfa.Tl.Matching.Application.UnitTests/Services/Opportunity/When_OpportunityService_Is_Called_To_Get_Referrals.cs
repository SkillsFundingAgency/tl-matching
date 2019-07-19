using System;
using System.Collections.Generic;
using System.Linq;
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
    public class When_OpportunityService_Is_Called_To_Get_Referrals
    {
        private readonly List<ReferralDto> _referralDtos;
        private readonly IRepository<Domain.Models.Referral> _referralRepository;

        private const int OpportunityItemId = 1;

        public When_OpportunityService_Is_Called_To_Get_Referrals()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);
            
            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityPipelineDto>>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

            _referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            _referralRepository.GetMany(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>()).Returns(
                new List<Domain.Models.Referral>
                {
                    new Domain.Models.Referral
                    {
                        ProviderVenue = new Domain.Models.ProviderVenue
                        {
                            Postcode = "AA1 1AA",
                            Provider = new Domain.Models.Provider
                            {
                                Name = "Provider1"
                            }
                        }
                    }
                }.AsQueryable());

            var opportunityService = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository, 
                provisionGapRepository, _referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            _referralDtos = opportunityService.GetReferrals(OpportunityItemId);
        }

        [Fact]
        public void Then_GetMany_Is_Called_Exactly_Once()
        {
            _referralRepository
                .Received(1)
                .GetMany(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>());
        }

        [Fact]
        public void Then_Fields_Are_Set_To_Expected_Values()
        {
            _referralDtos.Count.Should().Be(1);
            _referralDtos[0].Postcode.Should().Be("AA1 1AA");
            _referralDtos[0].Name.Should().Be("Provider1");
        }
    }
}