using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sfa.Tl.Matching.Tests.Common.Extensions
{
    public class FakeAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public FakeAsyncEnumerable(Expression expression) : base(expression) { }

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new FakeAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider => new FakeAsyncQueryProvider<T>(this);
    }
}