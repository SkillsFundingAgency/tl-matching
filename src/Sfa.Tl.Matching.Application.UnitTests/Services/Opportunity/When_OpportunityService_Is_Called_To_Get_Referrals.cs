using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Get_Referrals
    {
        private readonly List<ReferralsViewModel> _providerViewModels;
        private readonly IRepository<Domain.Models.Referral> _referralRepository;

        private const int OpportunityId = 1;

        public When_OpportunityService_Is_Called_To_Get_Referrals()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            _referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            _referralRepository.GetMany(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>(),
                Arg.Any<Expression<Func<Domain.Models.Referral, object>>>(),
                Arg.Any<Expression<Func<Domain.Models.Referral, object>>>()).Returns(
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

            var opportunityService = new OpportunityService(mapper, dateTimeProvider, opportunityRepository, provisionGapRepository, _referralRepository);

            _providerViewModels = opportunityService.GetReferrals(OpportunityId);
        }

        [Fact]
        public void Then_GetMany_Is_Called_Exactly_Once()
        {
            _referralRepository.GetMany(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>());
        }

        [Fact]
        public void Then_ViewModel_Count_Is_1()
        {
            _providerViewModels.Count.Should().Be(1);
        }

        [Fact]
        public void Then_Postcode_Is_Set()
        {
            _providerViewModels[0].Postcode.Should().Be("AA1 1AA");
        }

        [Fact]
        public void Then_Name_Is_Set()
        {
            _providerViewModels[0].Name.Should().Be("Provider1");
        }
    }
}