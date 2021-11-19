using System.Collections.Generic;
using FluentAssertions;
using Sfa.Tl.Matching.Domain.EqualityComparer;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Comparers.Referral
{
    public class When_All_Referrals_Are_Same
    {
        private readonly ReferralDifference.ReferralDifferenceDto _dto;

        public When_All_Referrals_Are_Same()
        {
            var viewModelReferrals = new List<Domain.Models.Referral>
            {
                new() { ProviderVenueId = 1, DistanceFromEmployer = 11 },
                new() { ProviderVenueId = 2, DistanceFromEmployer = 22 },
                new() { ProviderVenueId = 3, DistanceFromEmployer = 33 }
            };

            var databaseReferrals = new List<Domain.Models.Referral>
            {
                new() { ProviderVenueId = 1, DistanceFromEmployer = 1 },
                new() { ProviderVenueId = 2, DistanceFromEmployer = 2 },
                new() { ProviderVenueId = 3, DistanceFromEmployer = 3 }
            };

            var referralDifference = new ReferralDifference(new ReferralEqualityComparer());
            _dto = referralDifference.Get(viewModelReferrals, databaseReferrals);
        }

        [Fact]
        public void Then_Adds_Count_Is_0() =>
            _dto.Adds.Count.Should().Be(0);

        [Fact]
        public void Then_Deletes_Count_Is_0() =>
            _dto.Deletes.Count.Should().Be(0);

        [Fact]
        public void Then_Updates_Count_Is_3() =>
            _dto.Updates.Count.Should().Be(3);

        [Fact]
        public void Then_Updates_Index_0_ProviderVenueId_Is_Correct() =>
            _dto.Updates[0].ProviderVenueId.Should().Be(1);

        [Fact]
        public void Then_Updates_Index_0_DistanceFromEmployer_Is_Same() =>
            _dto.Updates[0].DistanceFromEmployer.Should().Be(1);

        [Fact]
        public void Then_Updates_Index_1_ProviderVenueId_Is_Correct() =>
            _dto.Updates[1].ProviderVenueId.Should().Be(2);

        [Fact]
        public void Then_Updates_Index_1_DistanceFromEmployer_Is_Same() =>
            _dto.Updates[1].DistanceFromEmployer.Should().Be(2);

        [Fact]
        public void Then_Updates_Index_2_ProviderVenueId_Is_Correct() =>
            _dto.Updates[2].ProviderVenueId.Should().Be(3);
        
        [Fact]
        public void Then_Updates_Index_2_DistanceFromEmployer_Is_Same() =>
            _dto.Updates[2].DistanceFromEmployer.Should().Be(3);
    }
}