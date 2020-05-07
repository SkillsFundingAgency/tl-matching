using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Domain.EqualityComparer
{
    // ReSharper disable once UnusedMember.Global
    public sealed class PostcodeLookupStagingEqualityComparer : IEqualityComparer<PostcodeLookupStaging>
    {
        public bool Equals(PostcodeLookupStaging x, PostcodeLookupStaging y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.Postcode, y.Postcode)
                   && string.Equals(x.PrimaryLepCode, y.PrimaryLepCode)
                   && string.Equals(x.SecondaryLepCode, y.SecondaryLepCode);
        }

        public int GetHashCode(PostcodeLookupStaging obj)
        {
            unchecked
            {
                var hashCode = (obj.Postcode != null ? obj.Postcode.GetHashCode() : 0);
                hashCode = (hashCode * 397)
                           ^ (obj.PrimaryLepCode != null ? obj.PrimaryLepCode.GetHashCode() : 0)
                           ^ (obj.SecondaryLepCode != null ? obj.SecondaryLepCode.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}