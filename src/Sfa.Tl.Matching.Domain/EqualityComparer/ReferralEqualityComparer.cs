using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Domain.EqualityComparer
{
    public sealed class ReferralEqualityComparer : IEqualityComparer<Referral>
    {
        public bool Equals(Referral x, Referral y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.OpportunityItemId == y.OpportunityItemId 
                   && x.ProviderVenueId == y.ProviderVenueId 
                ;
        }

        public int GetHashCode(Referral obj)
        {
            unchecked
            {
                var hashCode = obj.OpportunityItemId;
                hashCode = (hashCode * 397) ^ obj.ProviderVenueId;
                return hashCode;
            }
        }
    }
}