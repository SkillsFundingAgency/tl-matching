using System.Collections.Generic;
using FluentAssertions;
using Sfa.Tl.Matching.Domain.EqualityComparer;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Comparers.Referral
{
    public class When_One_Referral_Is_Added_And_Five_Deleted
    {
        private readonly ReferralDifference.ReferralDifferenceDto _dto;

        private const int OpportunityItemId = 1;

        public When_One_Referral_Is_Added_And_Five_Deleted()
        {
            var viewModelReferrals = new List<Domain.Models.Referral>
            {
                new Domain.Models.Referral { ProviderVenueId = 1, OpportunityItemId = OpportunityItemId },
                new Domain.Models.Referral { ProviderVenueId = 7, OpportunityItemId = OpportunityItemId }
            };

            var databaseReferrals = new List<Domain.Models.Referral>
            {
                new Domain.Models.Referral { ProviderVenueId = 1, OpportunityItemId = OpportunityItemId },
                new Domain.Models.Referral { ProviderVenueId = 2, OpportunityItemId = OpportunityItemId },
                new Domain.Models.Referral { ProviderVenueId = 3, OpportunityItemId = OpportunityItemId },
                new Domain.Models.Referral { ProviderVenueId = 4, OpportunityItemId = OpportunityItemId },
                new Domain.Models.Referral { ProviderVenueId = 5, OpportunityItemId = OpportunityItemId },
                new Domain.Models.Referral { ProviderVenueId = 6, OpportunityItemId = OpportunityItemId },
                new Domain.Models.Referral { ProviderVenueId = 6, OpportunityItemId = 2 }
            };

            var referralDifference = new ReferralDifference(new ReferralEqualityComparer());
            _dto = referralDifference.Get(viewModelReferrals, databaseReferrals);
        }

        [Fact]
        public void Then_Adds_Count_Is_1() =>
            _dto.Adds.Count.Should().Be(1);

        [Fact]
        public void Then_Deletes_Count_Is_6() =>
            _dto.Deletes.Count.Should().Be(6);

        [Fact]
        public void Then_Deletes_Index_0_ProviderVenueId_Is_Correct() =>
            _dto.Deletes[0].ProviderVenueId.Should().Be(2);

        [Fact]
        public void Then_Deletes_Index_1_ProviderVenueId_Is_Correct() =>
            _dto.Deletes[1].ProviderVenueId.Should().Be(3);

        [Fact]
        public void Then_Deletes_Index_2_ProviderVenueId_Is_Correct() =>
            _dto.Deletes[2].ProviderVenueId.Should().Be(4);

        [Fact]
        public void Then_Deletes_Index_3_ProviderVenueId_Is_Correct() =>
            _dto.Deletes[3].ProviderVenueId.Should().Be(5);

        [Fact]
        public void Then_Deletes_Index_4_ProviderVenueId_Is_Correct() =>
            _dto.Deletes[4].ProviderVenueId.Should().Be(6);

        [Fact]
        public void Then_Updates_Count_Is_1() =>
            _dto.Updates.Count.Should().Be(1);
    }
}