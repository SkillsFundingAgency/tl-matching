using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Domain.EqualityComparer
{
    // ReSharper disable once UnusedMember.Global
    public sealed class QualificationRouteMappingEqualityComparer : IEqualityComparer<QualificationRouteMapping>
    {
        public bool Equals(QualificationRouteMapping x, QualificationRouteMapping y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.RouteId == y.RouteId && x.QualificationId == y.QualificationId;
        }

        public int GetHashCode(QualificationRouteMapping obj)
        {
            unchecked
            {
                var hashCode = obj.RouteId;
                hashCode = (hashCode * 397) ^ obj.QualificationId;
                return hashCode;
            }
        }
    }
}