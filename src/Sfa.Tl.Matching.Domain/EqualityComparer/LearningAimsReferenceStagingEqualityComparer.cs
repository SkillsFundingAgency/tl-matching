using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public sealed class LearningAimsReferenceStagingEqualityComparer : IEqualityComparer<LearningAimsReferenceStaging>
    {
        public bool Equals(LearningAimsReferenceStaging x, LearningAimsReferenceStaging y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.LarId, y.LarId) && string.Equals(x.Title, y.Title) && string.Equals(x.AwardOrgLarId, y.AwardOrgLarId) && string.Equals(x.SourceCreatedOn, y.SourceCreatedOn) && string.Equals(x.SourceModifiedOn, y.SourceModifiedOn);
        }

        public int GetHashCode(LearningAimsReferenceStaging obj)
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