﻿using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Domain.EqualityComparer
{
    public sealed class QualificationRoutePathMappingEqualityComparer : IEqualityComparer<QualificationRoutePathMapping>
    {
        public bool Equals(QualificationRoutePathMapping x, QualificationRoutePathMapping y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.PathId == y.PathId && x.QualificationId == y.QualificationId && string.Equals(x.Source, y.Source) && Equals(x.Path, y.Path) && new QualificationEqualityComparer().Equals(x.Qualification, y.Qualification);
        }

        public int GetHashCode(QualificationRoutePathMapping obj)
        {
            unchecked
            {
                var hashCode = obj.PathId;
                hashCode = (hashCode * 397) ^ obj.QualificationId;
                hashCode = (hashCode * 397) ^ (obj.Source != null ? obj.Source.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Path != null ? obj.Path.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Qualification != null ? new QualificationEqualityComparer().GetHashCode(obj.Qualification) : 0);
                return hashCode;
            }
        }
    }
}