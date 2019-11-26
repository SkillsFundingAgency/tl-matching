using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Domain.EqualityComparer
{
    // ReSharper disable once UnusedMember.Global
    public sealed class ProviderQualificationEqualityComparer : IEqualityComparer<ProviderQualification>
    {
        public bool Equals(ProviderQualification x, ProviderQualification y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.ProviderVenueId == y.ProviderVenueId && x.QualificationId == y.QualificationId && string.Equals(x.Source, y.Source) && Equals(x.ProviderVenue, y.ProviderVenue) && Equals(x.Qualification, y.Qualification);
        }

        public int GetHashCode(ProviderQualification obj)
        {
            unchecked
            {
                var hashCode = obj.ProviderVenueId;
                hashCode = (hashCode * 397) ^ obj.QualificationId;
                hashCode = (hashCode * 397) ^ (obj.Source != null ? obj.Source.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.ProviderVenue != null ? obj.ProviderVenue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Qualification != null ? obj.Qualification.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}