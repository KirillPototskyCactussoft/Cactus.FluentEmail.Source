using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Cactus.FluentEmail.Source.EntityFraemwork.Tests
{
    internal class AsyncQueryProvider<T> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _items;

        public AsyncQueryProvider(AsyncEnumerable<T> items)
        {
            _items = items;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new AsyncEnumerable<T>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _items.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _items.Execute<TResult>(expression);
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new AsyncEnumerable<TResult>(expression);
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }
}
