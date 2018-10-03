using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FluentEmail.Source.EntityFraemwork.Tests
{
    internal class AsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IAsyncEnumerator<T>, IQueryable<T>
    {
        private readonly IEnumerator<T> _items;

        public AsyncEnumerable(IEnumerable<T> items) : base(items)
        {
            _items = items.GetEnumerator();
        }

        public AsyncEnumerable(Expression expression) : base(expression)
        {
            _items = this.AsEnumerable().GetEnumerator();
        }

        public T Current => _items.Current;

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return this;
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(_items.MoveNext());
        }

        public void Dispose()
        {
            _items.Dispose();
        }

        IQueryProvider IQueryable.Provider => new AsyncQueryProvider<T>(this);
    }
}
