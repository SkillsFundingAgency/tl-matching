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
    public class When_OpportunityService_Is_Called_To_Delete_Referral
    {
        private const int ReferralId = 2;
        private const int OpportunityItemId = 1;

        private readonly IRepository<Domain.Models.Referral> _referralRepository;

        public When_OpportunityService_Is_Called_To_Delete_Referral()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            _referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            _referralRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>())
                .Returns(new Domain.Models.Referral
                {
                    Id = ReferralId,
                    OpportunityItemId = OpportunityItemId
                });

            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityReportDto>>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

            var opportunityService = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository,
                provisionGapRepository, _referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            opportunityService.DeleteReferralAsync(ReferralId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Delete_Referral_Is_Called_Exactly_Once()
        {
            _referralRepository.Received(1).DeleteAsync(Arg.Any<Domain.Models.Referral>());
        }

        [Fact]
        public void Then_Delete_Referral_Is_Called_With_Expected_Values()
        {
            _referralRepository
                .Received()
                .DeleteAsync(Arg.Is<Domain.Models.Referral>(
                    r => r.Id == ReferralId));
        }
    }
}
