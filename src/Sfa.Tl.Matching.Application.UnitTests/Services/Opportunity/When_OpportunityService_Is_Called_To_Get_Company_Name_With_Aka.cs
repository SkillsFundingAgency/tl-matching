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
    public class When_OpportunityService_Is_Called_To_Get_Company_Name_With_Aka
    {
        private readonly string _result;
        private readonly IOpportunityRepository _opportunityRepository;

        public When_OpportunityService_Is_Called_To_Get_Company_Name_With_Aka()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityPipelineDto>>();

            var opportunity = new Domain.Models.Opportunity
            {
                Id = 1,
                Employer = new Domain.Models.Employer()
                {
                    CompanyName = "CompanyName",
                    AlsoKnownAs = "AlsoKnownAs"
                }
            };

            _opportunityRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, object>>>())
                .Returns(opportunity);

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, opportunityItemRepository, provisionGapRepository, referralRepository, googleMapApiClient, opportunityPipelineReportWriter);

            _result = opportunityService.GetCompanyNameWithAkaAsync(1).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_Opportunity_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetSingleOrDefault(
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, object>>>()
            );
        }

        [Fact]
        public void Then_Result_Is_Company_Name_With_Aka()
        {
            _result.Should().Be("CompanyName (AlsoKnownAs)");
        }
    }
}