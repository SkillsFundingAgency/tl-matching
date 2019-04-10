using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Domain.EqualityComparer;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests
{
    internal class ReferralDifference
    {
        internal class ReferralDifferenceDto
        {
            public List<Referral> Adds { get; }
            public List<Referral> Deletes { get; }
            public List<Referral> Updates { get; }

            public ReferralDifferenceDto(List<Referral> adds, List<Referral> deletes, List<Referral> updates)
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

        internal ReferralDifferenceDto Get(List<Referral> newReferrals, List<Referral> existingReferrals)
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