using System.Collections;

namespace Sfa.Tl.Matching.Application.Extensions
{
    public static class CollectionExtensions
    {
        // Summary:
        //     Checks whether or not collection is null or empty. Assumes collection can be
        //     safely enumerated multiple times.
        public static bool IsNullOrEmpty(this IEnumerable enumerable)
        {
            if (enumerable != null)
            {
                return !enumerable.GetEnumerator().MoveNext();
            }

            return true;
        }
    }
}