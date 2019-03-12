using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Create_Opportunity
    {
        private readonly int _result;
        private const int OpportunityId = 1;

        public When_OpportunityService_Is_Called_To_Create_Opportunity()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();
            var provisionGapRepository = Substitute.For<IRepository<Domain.Models.ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            opportunityRepository.Create(Arg.Any<Domain.Models.Opportunity>())
                .Returns(OpportunityId);

            var opportunityService = new OpportunityService(mapper, dateTimeProvider, opportunityRepository, provisionGapRepository, referralRepository);

            var dto = new OpportunityDto
            {
                EmployerContact = "Contact"
            };

            _result = opportunityService.CreateOpportunity(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityId_Is_Created()
        {
            _result.Should().Be(OpportunityId);
        }
    }
}