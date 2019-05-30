using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Domain.EqualityComparer
{
    public sealed class LearningAimReferenceStagingEqualityComparer : IEqualityComparer<LearningAimReferenceStaging>
    {
        public bool Equals(LearningAimReferenceStaging x, LearningAimReferenceStaging y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.LarId, y.LarId) && string.Equals(x.Title, y.Title) && string.Equals(x.AwardOrgLarId, y.AwardOrgLarId) && Equals(x.SourceCreatedOn, y.SourceCreatedOn) && Equals(x.SourceModifiedOn, y.SourceModifiedOn);
        }

        public int GetHashCode(LearningAimReferenceStaging obj)
        {
            unchecked
            {
                var hashCode = (obj.LarId != null ? obj.LarId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Title != null ? obj.Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.AwardOrgLarId != null ? obj.AwardOrgLarId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.SourceCreatedOn != null ? obj.SourceCreatedOn.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.SourceModifiedOn != null ? obj.SourceModifiedOn.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}