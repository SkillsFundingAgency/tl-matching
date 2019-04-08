using System.Collections.Generic;
using FluentAssertions;
using Sfa.Tl.Matching.Domain.EqualityComparer;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Comparers.Referral
{
    public class When_All_Referrals_Are_Deleted
    {
        private readonly ReferralDifference.ReferralDifferenceDto _dto;

        public When_All_Referrals_Are_Deleted()
        {
            var viewModelReferrals = new List<Domain.Models.Referral>();

            var databaseReferrals = new List<Domain.Models.Referral>
            {
                new Domain.Models.Referral { ProviderVenueId = 1 },
                new Domain.Models.Referral { ProviderVenueId = 2 },
                new Domain.Models.Referral { ProviderVenueId = 3 }
            };

            var referralDifference = new ReferralDifference(new ReferralEqualityComparer());
            _dto = referralDifference.Get(viewModelReferrals, databaseReferrals);
        }

        [Fact]
        public void Then_Adds_Count_Is_0() =>
            _dto.Adds.Count.Should().Be(0);

        [Fact]
        public void Then_Deletes_Count_Is_3() =>
            _dto.Deletes.Count.Should().Be(3);

        [Fact]
        public void Then_Deletes_Index_1_ProviderVenueId_Is_Correct() =>
            _dto.Deletes[0].ProviderVenueId.Should().Be(1);

        [Fact]
        public void Then_Deletes_Index_2_ProviderVenueId_Is_Correct() =>
            _dto.Deletes[1].ProviderVenueId.Should().Be(2);

        [Fact]
        public void Then_Deletes_Index_3_ProviderVenueId_Is_Correct() =>
            _dto.Deletes[2].ProviderVenueId.Should().Be(3);

        [Fact]
        public void Then_Updates_Count_Is_0() =>
            _dto.Updates.Count.Should().Be(0);
    }
}