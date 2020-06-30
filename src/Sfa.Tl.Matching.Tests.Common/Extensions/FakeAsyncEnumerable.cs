using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Sfa.Tl.Matching.Tests.Common.Extensions
{
    public class FakeAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public FakeAsyncEnumerable(Expression expression) : base(expression) { }

        public FakeAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }

        IQueryProvider IQueryable.Provider => new FakeAsyncQueryProvider<T>(this);

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new FakeAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator(), cancellationToken);
        }
    }
}