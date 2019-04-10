using System.Collections.Generic;
using FluentAssertions;
using Sfa.Tl.Matching.Domain.EqualityComparer;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Comparers.Referral
{
    public class When_Two_Referrals_Are_Added_And_Two_Deleted_And_Two_Updated
    {
        private readonly ReferralDifference.ReferralDifferenceDto _dto;

        public When_Two_Referrals_Are_Added_And_Two_Deleted_And_Two_Updated()
        {
            var viewModelReferrals = new List<Domain.Models.Referral>
            {
                new Domain.Models.Referral { ProviderVenueId = 1, DistanceFromEmployer = 1 },
                new Domain.Models.Referral { ProviderVenueId = 3, DistanceFromEmployer = 3 },
                new Domain.Models.Referral { ProviderVenueId = 6, DistanceFromEmployer = 6 },
                new Domain.Models.Referral { ProviderVenueId = 7, DistanceFromEmployer = 7 }
            };

            var databaseReferrals = new List<Domain.Models.Referral>
            {
                new Domain.Models.Referral { ProviderVenueId = 4, DistanceFromEmployer = 4 },
                new Domain.Models.Referral { ProviderVenueId = 5, DistanceFromEmployer = 5 },
                new Domain.Models.Referral { ProviderVenueId = 6, DistanceFromEmployer = 6 },
                new Domain.Models.Referral { ProviderVenueId = 7, DistanceFromEmployer = 7 }
            };

            var referralDifference = new ReferralDifference(new ReferralEqualityComparer());
            _dto = referralDifference.Get(viewModelReferrals, databaseReferrals);
        }

        [Fact]
        public void Then_Adds_Count_Is_2() =>
            _dto.Adds.Count.Should().Be(2);

        [Fact]
        public void Then_Adds_Index_1_ProviderVenueId_Is_Correct() =>
            _dto.Adds[0].ProviderVenueId.Should().Be(1);

        [Fact]
        public void Then_Adds_Index_2_ProviderVenueId_Is_Correct() =>
            _dto.Adds[1].ProviderVenueId.Should().Be(3);

        [Fact]
        public void Then_Deletes_Count_Is_2() =>
            _dto.Deletes.Count.Should().Be(2);

        [Fact]
        public void Then_Deletes_Index_0_ProviderVenueId_Is_Correct() =>
            _dto.Deletes[0].ProviderVenueId.Should().Be(4);

        [Fact]
        public void Then_Deletes_Index_1_ProviderVenueId_Is_Correct() =>
            _dto.Deletes[1].ProviderVenueId.Should().Be(5);

        [Fact]
        public void Then_Updates_Count_Is_2() =>
            _dto.Updates.Count.Should().Be(2);
    }
}