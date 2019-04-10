using System.Collections.Generic;
using FluentAssertions;
using Sfa.Tl.Matching.Domain.EqualityComparer;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Comparers.Referral
{
    public class When_One_Referral_Is_Added_And_One_Deleted
    {
        private readonly ReferralDifference.ReferralDifferenceDto _dto;

        public When_One_Referral_Is_Added_And_One_Deleted()
        {
            var viewModelReferrals = new List<Domain.Models.Referral>
            {
                new Domain.Models.Referral { ProviderVenueId = 1, DistanceFromEmployer = 1 },
                new Domain.Models.Referral { ProviderVenueId = 3, DistanceFromEmployer = 3 }
            };

            var databaseReferrals = new List<Domain.Models.Referral>
            {
                new Domain.Models.Referral { ProviderVenueId = 1, DistanceFromEmployer = 1 },
                new Domain.Models.Referral { ProviderVenueId = 2, DistanceFromEmployer = 2 }
            };

            var referralDifference = new ReferralDifference(new ReferralEqualityComparer());
            _dto = referralDifference.Get(viewModelReferrals, databaseReferrals);
        }

        [Fact]
        public void Then_Adds_Count_Is_1() =>
            _dto.Adds.Count.Should().Be(1);

        [Fact]
        public void Then_Adds_Index_0_ProviderVenueId_Is_Correct() =>
            _dto.Adds[0].ProviderVenueId.Should().Be(3);

        [Fact]
        public void Then_Deletes_Count_Is_1() =>
            _dto.Deletes.Count.Should().Be(1);

        [Fact]
        public void Then_Deletes_Index_0_ProviderVenueId_Is_Correct() =>
            _dto.Deletes[0].ProviderVenueId.Should().Be(2);

        [Fact]
        public void Then_Updates_Count_Is_1() =>
            _dto.Updates.Count.Should().Be(1);
    }
}