using System.Collections.Generic;
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
    public class When_OpportunityService_Is_Called_To_Update_Referrals
    {
        private readonly IRepository<Domain.Models.Referral> _referralRepository;

        private const int OpportunityId = 1;

        public When_OpportunityService_Is_Called_To_Update_Referrals()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            var opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            _referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            opportunityRepository.Create(Arg.Any<Domain.Models.Opportunity>())
                .Returns(OpportunityId);

            var opportunityService = new OpportunityService(mapper, opportunityRepository, provisionGapRepository, _referralRepository);

            var dto = new OpportunityDto
            {
                EmployerContact = "EmployerContact",
                Referral = new List<ReferralDto>
                {
                    new ReferralDto
                    {
                        ProviderVenueId = 1,
                        DistanceFromEmployer = 3.5M
                    }
                }
            };

            opportunityService.UpdateReferrals(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_DeleteMany_Is_Called_Exactly_Once()
        {
            _referralRepository.Received(1)
                .DeleteMany(Arg.Any<IList<Domain.Models.Referral>>());
        }

        [Fact]
        public void Then_CreateMany_Is_Called_Exactly_Once()
        {
            _referralRepository.Received(1)
                .CreateMany(Arg.Any<IList<Domain.Models.Referral>>());
        }

        [Fact]
        public void Then_UpdateMany_Is_Called_Exactly_Once()
        {
            _referralRepository.Received(1)
                .UpdateMany(Arg.Any<IList<Domain.Models.Referral>>());
        }
    }
}