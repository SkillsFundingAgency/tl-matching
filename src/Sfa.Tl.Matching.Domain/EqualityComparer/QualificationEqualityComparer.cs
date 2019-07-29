using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Domain.EqualityComparer
{
    public sealed class QualificationEqualityComparer : IEqualityComparer<Qualification>
    {
        public bool Equals(Qualification x, Qualification y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.LarsId, y.LarsId) && string.Equals(x.Title, y.Title) && string.Equals(x.ShortTitle, y.ShortTitle) && Equals(x.ProviderQualification, y.ProviderQualification) && Equals(x.QualificationRouteMapping, y.QualificationRouteMapping);
        }

        public int GetHashCode(Qualification obj)
        {
            unchecked
            {
                var hashCode = (obj.LarsId != null ? obj.LarsId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Title != null ? obj.Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.ShortTitle != null ? obj.ShortTitle.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.ProviderQualification != null ? obj.ProviderQualification.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.QualificationRouteMapping != null ? obj.QualificationRouteMapping.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}