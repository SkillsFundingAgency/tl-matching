using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Domain.EqualityComparer;

namespace Sfa.Tl.Matching.Application.UnitTests.Comparers.Referral
{
    internal class ReferralDifference
    {
        internal class ReferralDifferenceDto
        {
            public List<Domain.Models.Referral> Adds { get; }
            public List<Domain.Models.Referral> Deletes { get; }
            public List<Domain.Models.Referral> Updates { get; }

            public ReferralDifferenceDto(List<Domain.Models.Referral> adds, List<Domain.Models.Referral> deletes, List<Domain.Models.Referral> updates)
            {
                Adds = adds;
                Deletes = deletes;
                Updates = updates;
            }
        }

        private readonly ReferralEqualityComparer _comparer;

        public ReferralDifference(ReferralEqualityComparer comparer)
        {
            _comparer = comparer;
        }

        internal ReferralDifferenceDto Get(List<Domain.Models.Referral> newReferrals, List<Domain.Models.Referral> existingReferrals)
        {
            var toBeAdded = newReferrals.Except(existingReferrals, _comparer).ToList();
            var same = existingReferrals.Intersect(newReferrals, _comparer).ToList();
            var toBeDeleted = existingReferrals.Except(same).ToList();

            var referralDifferenceDto = new ReferralDifferenceDto(toBeAdded,
                toBeDeleted, same);

            return referralDifferenceDto;
        }
    }
}