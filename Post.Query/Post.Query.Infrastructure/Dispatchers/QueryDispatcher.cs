﻿using CQRS.Core.Infrastructure;
using CQRS.Core.Queries;
using Post.Query.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher<PostEntity>
    {
        private readonly Dictionary<Type, Func<BaseQuery, Task<List<PostEntity>>>> _handlers = new();
        public void RegisterHandler<TQuery>(Func<TQuery, Task<List<PostEntity>>> handler) where TQuery : BaseQuery
        {
            if (_handlers.ContainsKey(typeof(TQuery)))
            {
                throw new IndexOutOfRangeException("Can not register the same query handler twice");
            }
            _handlers.Add(typeof(TQuery), x => handler((TQuery)x));
        }

        public async Task<List<PostEntity>> SendAsync(BaseQuery query)
        {
            if (_handlers.TryGetValue(query.GetType(), out var handler))
            {
                return await handler(query);
            }
            throw new ArgumentNullException(nameof(handler),"Can not find the handler of "); ;
        }
    }
}
