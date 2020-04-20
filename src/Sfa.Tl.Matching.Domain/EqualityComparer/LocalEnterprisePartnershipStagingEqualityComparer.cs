using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Domain.EqualityComparer
{
    // ReSharper disable once UnusedMember.Global
    public sealed class LocalEnterprisePartnershipStagingEqualityComparer : IEqualityComparer<LocalEnterprisePartnershipStaging>
    {
        public bool Equals(LocalEnterprisePartnershipStaging x, LocalEnterprisePartnershipStaging y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.Code, y.Code) && string.Equals(x.Name, y.Name);
        }

        public int GetHashCode(LocalEnterprisePartnershipStaging obj)
        {
            unchecked
            {
                var hashCode = (obj.Code != null ? obj.Code.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Name != null ? obj.Name.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}