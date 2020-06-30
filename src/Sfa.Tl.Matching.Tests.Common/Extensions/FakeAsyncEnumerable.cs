using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Sfa.Tl.Matching.Tests.Common.Extensions
{
    public class FakeAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public FakeAsyncEnumerable(Expression expression) : base(expression) { }

        public FakeAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }

        IQueryProvider IQueryable.Provider => new FakeAsyncQueryProvider<T>(this);
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return new FakeAsyncEnumerator<T>(this.AsAsyncEnumerable().GetAsyncEnumerator());
        }
    }
}