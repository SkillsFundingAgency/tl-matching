using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class FakeAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public FakeAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }

        public FakeAsyncEnumerable(Expression expression) : base(expression) { }

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new FakeAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider => new FakeAsyncQueryProvider<T>(this);
    }
}