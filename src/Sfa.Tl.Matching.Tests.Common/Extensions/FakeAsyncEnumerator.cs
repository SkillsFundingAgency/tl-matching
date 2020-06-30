using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Tests.Common.Extensions
{
    public class FakeAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        private readonly CancellationToken _cancellationToken;

        public FakeAsyncEnumerator(IEnumerator<T> enumerator, CancellationToken cancellationToken = default)
        {
            _enumerator = enumerator;
            _cancellationToken = cancellationToken;
        }
        
        public T Current => _enumerator.Current;

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_enumerator.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            _enumerator.Dispose();
            return new ValueTask();
        }
    }
}