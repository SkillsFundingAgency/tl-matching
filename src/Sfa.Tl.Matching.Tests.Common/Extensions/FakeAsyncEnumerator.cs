using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Tests.Common.Extensions
{
    public class FakeAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IAsyncEnumerator<T> _inner;

        public FakeAsyncEnumerator(IAsyncEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.DisposeAsync();
        }

        public T Current => _inner.Current;

        public ValueTask<bool> MoveNextAsync()
        {
            return _inner.MoveNextAsync();
        }

        public ValueTask DisposeAsync()
        {
            return _inner.DisposeAsync();
        }
    }
}