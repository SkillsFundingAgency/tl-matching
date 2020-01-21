using System;
using System.Collections;

namespace Sfa.Tl.Matching.Tests.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty(this IEnumerable @this)
        {
            return @this == null || !@this.GetEnumerator().MoveNext();
        }
    }
}
