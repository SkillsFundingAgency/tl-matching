using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Domain.EqualityComparer
{
    // ReSharper disable once UnusedMember.Global
    public sealed class QualificationRoutePathMappingEqualityComparer : IEqualityComparer<QualificationRoutePathMapping>
    {
        public bool Equals(QualificationRoutePathMapping x, QualificationRoutePathMapping y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.RouteId == y.RouteId && x.QualificationId == y.QualificationId;
            //&& Equals(x.Route, y.Route) && new QualificationEqualityComparer().Equals(x.Qualification, y.Qualification);
        }

        public int GetHashCode(QualificationRoutePathMapping obj)
        {
            unchecked
            {
                var hashCode = obj.RouteId;
                hashCode = (hashCode * 397) ^ obj.QualificationId;
                //hashCode = (hashCode * 397) ^ (obj.Route != null ? obj.Route.GetHashCode() : 0);
                //hashCode = (hashCode * 397) ^ (obj.Qualification != null ? new QualificationEqualityComparer().GetHashCode(obj.Qualification) : 0);
                return hashCode;
            }
        }
    }
}