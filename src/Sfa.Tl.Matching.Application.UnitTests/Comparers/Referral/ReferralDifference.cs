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

        internal ReferralDifferenceDto Get(List<Referral> target, List<Referral> source)
        {
            var toUpdate = source.Intersect(target, _comparer).ToList();
            var toAdd = target.Except(toUpdate, _comparer).ToList();
            var toDelete = source.Except(toUpdate, _comparer).ToList();

            var referralDifferenceDto = new ReferralDifferenceDto(toAdd,
                toDelete, toUpdate);

            return referralDifferenceDto;
        }
    }
}